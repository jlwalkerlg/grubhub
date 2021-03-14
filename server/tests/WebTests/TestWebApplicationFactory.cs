using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web;
using Web.Data.EF;
using Web.Features.Billing;
using Web.Services.Geocoding;
using Web.Workers;
using WebTests.Doubles;
using WebTests.TestData;

namespace WebTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            // Executed after Startup.ConfigureServices.
            // This is a breaking change for the Generic Host with ASP.NET Core 3.0.
            // Before ASP.NET Core 3.0, this was executed before Startup.ConfigureServices.
            // For systems still using Web Host instead of Generic Host, this
            // is executed before Startup.ConfigureServices.
            // Can be used to override other services registered in Startup.
            builder.ConfigureServices((ctx, services) =>
            {
                var appSettings = ctx.Configuration.GetSection("App").Get<AppSettings>() with
                {
                    Environment = "Testing",
                };

                var databaseSettings = ctx.Configuration.GetSection("Database").Get<DatabaseSettings>() with
                {
                    ConnectionString = ctx.Configuration.GetSection("Database").GetValue<string>("TestConnectionString"),
                };

                services.AddSingleton(appSettings);
                services.AddSingleton(databaseSettings);

                services.Remove(
                    services.Single(x => x.ImplementationType == typeof(EventWorker))
                );

                services.Remove(
                    services.Single(x => x.ServiceType == typeof(IHostedService) &&
                        (x.ImplementationType?.Namespace.StartsWith("Quartz") ?? false))
                );

                // Set the default authentication scheme to "Test",
                // and register the handler for it.
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, AuthHandlerFake>(
                        "Test", options => { });

                services.AddLogging(logging =>
                {
                    logging.AddFilter(
                        typeof(WebTests.Doubles.AuthHandlerFake).FullName,
                        LogLevel.Warning);
                });

                services.Remove(
                    services.Single(x => x.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>))
                );

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(databaseSettings.ConnectionString, b =>
                    {
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                });

                services.AddDbContext<TestDbContext>(options =>
                {
                    options.UseNpgsql(databaseSettings.ConnectionString, b =>
                    {
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                });

                services.AddSingleton<IBillingService, BillingServiceSpy>();

                services.AddSingleton<IGeocoder, GeocoderStub>();
            });

            // Also executed after Startup.ConfigureServices.
            // Only necessary if a version earlier than ASP.NET Core 3.0,
            // or still using the Web Host instead of the Generic Host,
            // otherwise redundant.
            builder.ConfigureTestServices(services =>
            {
            });
        }
    }
}
