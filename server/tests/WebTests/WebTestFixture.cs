using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Application.Services;
using Application.Services.Geocoding;
using Web;
using WebTests.Doubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedTests.Doubles;

namespace WebTests
{
    public class WebTestFixture
    {
        public virtual WebTestAppFactory Factory { get; } = new();

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

    public class WebTestAppFactory : WebApplicationFactory<Startup>
    {
        protected virtual WebTestServiceProviderFactory ServiceProviderFactory { get; } = new();

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

    public class WebTestServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
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

            builder
                .RegisterType<ClockStub>()
                .As<IClock>()
                .SingleInstance();
        }
    }
}
