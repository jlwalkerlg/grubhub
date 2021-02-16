using Autofac;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using Web.Services.Antiforgery;
using Web.Services.Authentication;
using Web.Services.Hashing;
using Web.Services.Jobs;
using Web.Services.Notifications;

namespace Web
{
    public class Startup
    {
        public readonly Config config = new();
        private readonly IHostEnvironment env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            configuration.Bind(config);
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(config);

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
                        .WithOrigins(config.CorsOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();

            services.AddEntityFramework(config);

            services.AddMediatR(typeof(Startup).Assembly);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddSingleton<IClock, Clock>();

            services.AddStripe(config);

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddSingleton<INotifier, HubNotifier>();

            services.AddScoped<EventDispatcher>();
            services.AddScoped<IJobQueue, MediatRJobQueue>();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN"; // as expected from the client
                options.Cookie.Name = "csrf_token"; // set automatically by asp.net as http only
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddGeocoder();
            builder.AddMiddleware();

            builder.RegisterType<DbConnectionFactory>()
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery, ILogger<Startup> logger)
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
                    // TODO: uncomment in production
                    // Secure = CookieSecurePolicy.Always,
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
