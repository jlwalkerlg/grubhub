using System;
using System.Linq;
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
    public class WebAppFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new TestServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((webHostBuilderContext, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                    options.SuppressStatusMessages = true);

                RemoveService<DbContextOptions<AppDbContext>>(services);
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDB"));
            });
        }

        private void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services
                .SingleOrDefault(
                    d => d.ServiceType == typeof(T));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }

    public class TestServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
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

    [CollectionDefinition("WebApp")]
    public class WebAppCollectionFixture : ICollectionFixture<WebAppFactory>
    {
    }
}
