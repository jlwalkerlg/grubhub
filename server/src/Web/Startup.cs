using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using DotNetCore.CAP;
using Web.Data;
using Web.Filters;
using Web.Hubs;
using Web.Services.Antiforgery;
using Web.Services.Authentication;
using Web.Services.Hashing;
using Web.Services.Validation;
using Web.Services.Geocoding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Savorboard.CAP.InMemoryMessageQueue;
using Web.Data.EF;
using Web.Services;
using Web.Services.Billing;
using Web.Services.Cache;
using Web.Services.DateTimeServices;
using Web.Services.Events;
using Web.Services.Mail;
using Web.Services.Storage;

namespace Web
{
    public class Startup
    {
        private readonly IHostEnvironment env;
        private readonly Settings settings;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            this.env = env;
            settings = configuration.Get<Settings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(settings.App);
            services.AddSingleton(settings.Geocoding);
            services.AddSingleton(settings.Database);
            services.AddSingleton(settings.Stripe);
            services.AddSingleton(settings.Mail);
            services.AddSingleton(settings.Aws);
            services.AddSingleton(settings.Cache);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(settings.App.CorsOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ExceptionFilter>();

                    if (env.IsProduction())
                    {
                        options.Filters.Add<AntiforgeryFilter>();
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN"; // as expected from the client
                options.Cookie.Name = "csrf_token"; // set automatically by asp.net as http only
            });

            services.AddHttpContextAccessor();

            services.AddAuth(env);

            services.AddMediatR();

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            services.AddEntityFramework(settings.Database);

            services.AddDapper();

            services.AddDateTimeProvider();

            services.AddStripe(settings.Stripe);

            services.AddCap(settings);

            services.AddGeocoding();

            services.AddHashing();

            services.AddMail();

            services.AddImageStorage();

            services.AddDistributedCache(settings.Cache);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddValidators();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors();

            app.UseRouting();

            app.UseCookiePolicy(
                new CookiePolicyOptions()
                {
                    Secure = env.IsProduction()
                        ? CookieSecurePolicy.Always
                        : CookieSecurePolicy.None,
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OrderHub>("/hubs/orders");
            });
        }
    }
}
