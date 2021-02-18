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
            builder.ConfigureServices((ctx, services) =>
            {
                var config = new Config();
                ctx.Configuration.Bind(config);

                config = config with
                {
                    DbConnectionString = ctx.Configuration
                        .GetSection("TestDbConnectionString")
                        .Value,
                };

                services.AddSingleton(config);

                services.Remove(
                    services.Single(x => x.ImplementationType == typeof(EventWorker))
                );
                services.Remove(
                    services.Single(x => x.ImplementationType == typeof(JobWorker))
                );

                // Set the default authentication scheme to "Test",
                // and register the handler for it.
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, AuthHandlerFake>(
                        "Test", options => { });

                services.AddLogging(builder =>
                {
                    builder.AddFilter(
                        typeof(WebTests.Doubles.AuthHandlerFake).FullName,
                        LogLevel.Warning);
                });

                services.Remove(
                    services.Single(x => x.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>))
                );

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(config.DbConnectionString, b =>
                    {
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                });

                services.AddDbContext<TestDbContext>(options =>
                {
                    options.UseNpgsql(config.DbConnectionString, b =>
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
            // otherwise rudundant.
            builder.ConfigureTestServices(services =>
            {
            });
        }
    }
}
