using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Web.Data;
using Web.Filters;
using Web.Hubs;
using Web.Services.Antiforgery;
using Web.Services.Authentication;
using Web.Services.Hashing;
using Web.Services.Validation;
using Web.Services.Geocoding;
using Microsoft.AspNetCore.Http;
using Web.Data.EF;
using Web.Services;
using Web.Services.Billing;
using Web.Services.DateTimeServices;
using Web.Services.Events;
using Web.Services.Mail;
using Web.Services.Storage;

namespace Web
{
    public class Startup
    {
        private readonly IHostEnvironment env;
        private readonly Settings settings = new();

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            configuration.Bind(settings);
            env.EnvironmentName = settings.App.Environment;

            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(settings.App);
            services.AddSingleton(settings.Geocoding);
            services.AddSingleton(settings.Database);
            services.AddSingleton(settings.Stripe);
            services.AddSingleton(settings.Mail);
            services.AddSingleton(settings.Aws);

            services.AddLogging(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Information);
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

            services.AddQuartz(settings.Database);
            services.AddQuartzEventBus();
            services.AddEventProcessor();

            services.AddGeocoding();

            services.AddHashing();

            services.AddMail();

            services.AddImageStorage();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddValidators();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
