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
using Web.Services.Jobs;
using Web.Data.EF;
using Web.Services.Billing;
using Web.Services.DateTimeServices;
using Web.Services.Mail;

namespace Web
{
    public class Startup
    {
        private readonly IHostEnvironment env;
        private readonly AppSettings appSettings;
        private readonly GeocodingSettings geocodingSettings;
        private readonly DatabaseSettings databaseSettings;
        private readonly StripeSettings stripeSettings;
        private readonly MailSettings mailSettings;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            appSettings = configuration.GetSection("App").Get<AppSettings>();
            geocodingSettings = configuration.GetSection("Geocoding").Get<GeocodingSettings>();
            databaseSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
            stripeSettings = configuration.GetSection("Stripe").Get<StripeSettings>();
            mailSettings = configuration.GetSection("Mail").Get<MailSettings>();

            env.EnvironmentName = appSettings.Environment;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(appSettings);
            services.AddSingleton(geocodingSettings);
            services.AddSingleton(databaseSettings);
            services.AddSingleton(stripeSettings);
            services.AddSingleton(mailSettings);

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
                        .WithOrigins(appSettings.CorsOrigins)
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

            services.AddEntityFramework(databaseSettings);

            services.AddDapper();

            services.AddDateTimeProvider();

            services.AddStripe(stripeSettings);

            services.AddQuartz(databaseSettings);

            services.AddEventWorker();

            services.AddGeocoding();

            services.AddHashing();

            services.AddMail();
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
