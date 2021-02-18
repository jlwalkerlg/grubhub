using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Billing.SetupBilling
{
    public class SetupBillingActionTests : ActionTestBase
    {
        public SetupBillingActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/billing/setup");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/billing/setup");

            response.StatusCode.ShouldBe(400);
        }
    }
}
