using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using Xunit;

namespace FoodSnap.WebTests
{
    [CollectionDefinition(nameof(WebAppTestFixture))]
    public class WebAppTestCollection : ICollectionFixture<WebAppTestFixture>
    {
    }

    public class WebAppTestFixture
    {
        private readonly TestWebAppFactory factory;
        private readonly WebConfig config;
        private readonly Checkpoint checkpoint;

        public WebAppTestFixture()
        {
            factory = new TestWebAppFactory();

            config = factory.Services.GetRequiredService<WebConfig>();

            using (var db = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>())
            {
                db.Database.EnsureCreated();
            }

            checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[]
                {
                    "public"
                },
            };
        }

        public TestWebAppFactory Factory => factory;

        public async Task ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(config.DbConnectionString))
            {
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }

        public async Task ExecuteScope(Func<IServiceScope, Task> action)
        {
            using (var scope = factory.Services.CreateScope())
            {
                await action(scope);
            }
        }

        public async Task ExecuteService<TService>(Func<TService, Task> action)
        {
            await ExecuteScope(async scope =>
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await action(service);
            });
        }

        public async Task InsertDb(params object[] entities)
        {
            await ExecuteService<AppDbContext>(async db =>
            {
                await db.AddRangeAsync(entities);
                await db.SaveChangesAsync();
            });
        }
    }

    public class TestWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new TestWebAppServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                options.SuppressStatusMessages = true);

                var sp = services.BuildServiceProvider();

                var config = sp.GetRequiredService<WebConfig>();
                config.DbConnectionString = ctx.Configuration["TestDbConnectionString"];
            });
        }

        private void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }

    public class TestWebAppServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder builder)
        {
            ConfigureContainer(builder);

            return new AutofacServiceProvider(builder.Build());
        }

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<GeocoderStub>()
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
