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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace FoodSnap.WebTests
{
    [CollectionDefinition(nameof(StubbedWebAppTestFixture))]
    public class StubbedWebAppTestCollection : ICollectionFixture<StubbedWebAppTestFixture>
    {
    }

    public class StubbedWebAppTestFixture
    {
        private readonly StubbedTestWebAppFactory factory;

        public StubbedWebAppTestFixture()
        {
            factory = new StubbedTestWebAppFactory();
        }

        public StubbedTestWebAppFactory Factory => factory;

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
    }

    public class StubbedTestWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new StubbedTestWebAppServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                    options.SuppressStatusMessages = true);

                RemoveService<DbContextOptions<AppDbContext>>(services);
                services.AddDbContext<AppDbContext>(builder =>
                {
                    builder.UseInMemoryDatabase(nameof(AppDbContext));
                });
            });
        }

        private void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(T));
            services.Remove(descriptor);
        }
    }

    public class StubbedTestWebAppServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
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

            builder
                .RegisterGeneric(typeof(FailMiddlewareStub<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
