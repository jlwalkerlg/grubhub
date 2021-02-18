using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web;
using Xunit;

namespace WebTests.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingActionTests : ActionTestBase
    {
        private readonly Config config;

        public RefreshOnboardingActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
            config = factory.Server.Services.GetRequiredService<Config>();
        }

        [Fact]
        public async Task It_Redirects_To_The_Login_Page_If_Unauthenticated()
        {
            var uri = $"/stripe/onboarding/refresh?restaurant_id={Guid.NewGuid()}";
            var response = await GetClient().Get(uri);

            response.StatusCode.ShouldBe(302);
            response.Headers.Location.OriginalString.ShouldBe(
                $"{config.ClientUrl}/login?redirect_to={config.ServerUrl}{uri}");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await GetAuthenticatedClient().Get(
                $"/stripe/onboarding/refresh?restaurant_id={Guid.NewGuid()}");

            response.StatusCode.ShouldBe(400);
        }
    }
}
