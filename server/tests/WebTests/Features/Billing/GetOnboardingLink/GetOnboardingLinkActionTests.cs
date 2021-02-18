using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Billing.GetOnboardingLink
{
    public class GetOnboardingLinkActionTests : ActionTestBase
    {
        public GetOnboardingLinkActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await GetClient().Get(
                $"/restaurants/{Guid.NewGuid()}/billing/onboarding/link");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await GetAuthenticatedClient().Get(
                $"/restaurants/{Guid.NewGuid()}/billing/onboarding/link");

            response.StatusCode.ShouldBe(400);
        }
    }
}
