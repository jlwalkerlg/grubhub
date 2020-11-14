using Autofac;
using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodSnap.WebTests.Actions
{
    public class WebActionTestFixture : WebAppTestFixture
    {
        public override TestWebAppFactory Factory { get; } = new();
    }

    public class ActionTestWebAppFactory : TestWebAppFactory
    {
        protected override TestWebAppServiceProviderFactory ServiceProviderFactory { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices((ctx, services) =>
            {
                RemoveService<DbContextOptions<AppDbContext>>(services);
                services.AddDbContext<AppDbContext>(builder =>
                {
                    builder.UseInMemoryDatabase(nameof(AppDbContext));
                });
            });
        }
    }

    public class ActionTestWebAppServiceProviderFactory : TestWebAppServiceProviderFactory
    {
        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder
                .RegisterGeneric(typeof(FailMiddlewareStub<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
