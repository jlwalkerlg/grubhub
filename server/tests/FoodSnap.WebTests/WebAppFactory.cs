using System.Linq;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Web;
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
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((webHostBuilderContext, services) =>
            {
                var config = webHostBuilderContext.Configuration;

                // Suppress info log.
                services.Configure<ConsoleLifetimeOptions>(options =>
                    options.SuppressStatusMessages = true);

                // Remove the AppDbContext registration.
                var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register AppDbContext using the test database.
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDB"));
            });
        }
    }

    [CollectionDefinition("WebApp")]
    public class WebAppCollectionFixture : ICollectionFixture<WebAppFactory>
    {
    }
}
