using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Xunit;

namespace WebTests.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingActionTests : HttpTestBase
    {
        private readonly Config config;

        public RefreshOnboardingActionTests(HttpTestFixture fixture) : base(fixture)
        {
            config = fixture.GetService<Config>();
        }

        [Fact]
        public async Task It_Redirects_To_The_Login_Page_If_Unauthenticated()
        {
            var uri = $"/stripe/onboarding/refresh?restaurant_id={Guid.NewGuid()}";
            var response = await fixture.GetClient().Get(uri);

            response.StatusCode.ShouldBe(302);
            response.Headers.Location.OriginalString.ShouldBe(
                $"{config.ClientUrl}/login?redirect_to={config.ServerUrl}{uri}");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Get(
                $"/stripe/onboarding/refresh?restaurant_id={Guid.NewGuid()}");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
