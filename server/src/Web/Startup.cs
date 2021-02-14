using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Web.Data;
using Web.Features.Restaurants.SearchRestaurants;
using Web.Filters;
using Web.Hubs;
using Web.ServiceRegistration;
using Web.Services;
using Web.Services.Authentication;
using Web.Services.Hashing;
using Web.Services.Notifications;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.Bind(Config);
        }

        public Config Config { get; } = new();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Config);

            services.AddLogging(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Information);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_cookie";
                });

            services
                .AddControllers(options =>
                {
                    options.Filters.Add(new ExceptionFilter());
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
                        .WithOrigins(Config.CorsOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();

            services.AddEntityFramework(Config);

            services.AddMediatR(typeof(Startup).Assembly);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddSingleton<IClock, Clock>();

            services.AddStripe(Config);

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddSingleton<INotifier, Notifier>();

            services.AddScoped<EventProcessor>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddGeocoder();
            builder.AddMiddleware();

            builder.Register(ctx => new DbConnectionFactory(Config.DbConnectionString))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DPRestaurantSearcher>()
                .AsImplementedInterfaces();

            builder.RegisterType<Hasher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Authenticator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
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
                    MinimumSameSitePolicy = SameSiteMode.Strict,
                    HttpOnly = HttpOnlyPolicy.Always,
                    Secure = CookieSecurePolicy.None,
                });

            // TODO: how to put this in test server?
            if (env.IsTesting())
            {
                app.UseMiddleware<AuthenticationTestMiddleware>();
            }

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
