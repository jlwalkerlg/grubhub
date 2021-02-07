using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Billing.GenerateOnboardingLink
{
    public class GenerateOnboardingLinkActionTests : HttpTestBase
    {
        public GenerateOnboardingLinkActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Get(
                $"/restaurants/{Guid.NewGuid()}/billing/onboarding/link");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Get(
                $"/restaurants/{Guid.NewGuid()}/billing/onboarding/link");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
