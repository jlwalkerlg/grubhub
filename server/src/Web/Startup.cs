using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Web.Data;
using Web.Features.Restaurants.SearchRestaurants;
using Web.ServiceRegistration;
using Web.Services;
using Web.Services.Authentication;
using Web.Services.Cookies;
using Web.Services.Hashing;
using Web.Services.Tokenization;

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

            services
                .AddControllers()
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddGeocoder(Config);
            builder.AddMiddleware();

            builder.Register(ctx => new DbConnectionFactory(Config.DbConnectionString))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DPRestaurantSearcher>()
                .AsImplementedInterfaces();

            builder.RegisterType<Hasher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(ctx => new JWTTokenizer(Config.JWTSecret))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CookieBag>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
