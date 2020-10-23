using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Web;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodSnap.WebTests
{
    public class WebAppTestFixture
    {
        public virtual TestWebAppFactory Factory { get; } = new TestWebAppFactory();

        public async Task ExecuteScope(Func<IServiceScope, Task> action)
        {
            using (var scope = Factory.Services.CreateScope())
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

    public class TestWebAppFactory : WebApplicationFactory<Startup>
    {
        protected virtual TestWebAppServiceProviderFactory ServiceProviderFactory { get; } = new TestWebAppServiceProviderFactory();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(ServiceProviderFactory);
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((ctx, services) =>
            {
                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                options.SuppressStatusMessages = true);
            });
        }

        protected void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(T));
            services.Remove(descriptor);
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

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<GeocoderStub>()
                .As<IGeocoder>()
                .SingleInstance();
        }
    }
}
