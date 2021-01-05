using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Data.EF;
using WebTests.Doubles;
using Xunit;

namespace WebTests
{
    public class WebActionTestBase : WebTestBase, IClassFixture<WebActionTestFixture>
    {
        public WebActionTestBase(WebActionTestFixture fixture) : base(fixture)
        {
        }
    }

    public class WebActionTestFixture : WebTestFixture
    {
        public override WebTestAppFactory Factory { get; } = new WebActionTestAppFactory();
    }

    public class WebActionTestAppFactory : WebTestAppFactory
    {
        protected override WebTestServiceProviderFactory ServiceProviderFactory { get; } = new WebActionTestServiceProviderFactory();

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

    public class WebActionTestServiceProviderFactory : WebTestServiceProviderFactory
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
