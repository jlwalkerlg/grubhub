using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebTests.Doubles;

namespace WebTests
{
    public class ActionTestWebApplicationFactory : TestWebApplicationFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                // Since this is added after all other behaviors,
                // it will be executed last.
                // Short circuits the pipeline so that the handlers
                // aren't called, and a stubbed error is returned instead.
                services.AddTransient(
                    typeof(IPipelineBehavior<,>),
                    typeof(BadRequestBehaviorStub<,>)
                );
            });
        }
    }
}
