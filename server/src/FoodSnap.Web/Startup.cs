using System.Text.Json;
using Autofac;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Infrastructure.Hashing;
using FoodSnap.Infrastructure.Persistence;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web.ServiceRegistration;
using FoodSnap.Web.Services.Authentication;
using FoodSnap.Web.Services.Cookies;
using FoodSnap.Web.Services.Tokenization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodSnap.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.Bind(WebConfig);
        }

        public WebConfig WebConfig { get; } = new WebConfig();

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

            services.AddMediatR(
                typeof(FoodSnap.Application.IRequest).Assembly,
                typeof(Startup).Assembly);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddGeocoder(WebConfig);
            builder.AddMiddleware();

            builder.Register(ctx => new DbConnectionFactory(WebConfig.DbConnectionString))
                .As<IDbConnectionFactory>()
                .SingleInstance();

            builder.RegisterType<Hasher>()
                .As<IHasher>()
                .SingleInstance();

            builder.Register(ctx => new JWTTokenizer(WebConfig.JWTSecret))
                .As<ITokenizer>()
                .SingleInstance();

            builder.RegisterType<CookieBag>()
                .As<ICookieBag>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Authenticator>()
                .As<IAuthenticator>()
                .InstancePerLifetimeScope();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var scope = app.ApplicationServices.CreateScope())
                using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
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
