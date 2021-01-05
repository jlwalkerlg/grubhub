using System.Text.Json;
using Autofac;
using Web.Services.Hashing;
using Web.Data;
using Web.ServiceRegistration;
using Web.Services;
using Web.Services.Authentication;
using Web.Services.Cookies;
using Web.Services.Tokenization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.Bind(WebConfig);
        }

        public Config WebConfig { get; } = new();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(WebConfig);

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
                        .WithOrigins(WebConfig.CorsOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();

            services.AddEntityFramework(WebConfig);

            services.AddMediatR(typeof(Startup).Assembly);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddGeocoder(WebConfig);
            builder.AddMiddleware();

            builder.Register(ctx => new DbConnectionFactory(WebConfig.DbConnectionString))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.AddDtoRepositories();

            builder.RegisterType<Hasher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(ctx => new JWTTokenizer(WebConfig.JWTSecret))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CookieBag>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<Authenticator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<Clock>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // using (var scope = app.ApplicationServices.CreateScope())
                // using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                // {
                //     context.Database.EnsureDeleted();
                //     context.Database.EnsureCreated();
                // }
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
