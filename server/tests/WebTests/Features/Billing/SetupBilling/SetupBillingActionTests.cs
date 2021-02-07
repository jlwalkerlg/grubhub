using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Billing.SetupBilling
{
    public class SetupBillingActionTests : HttpTestBase
    {
        public SetupBillingActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/billing/setup");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/billing/setup");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
