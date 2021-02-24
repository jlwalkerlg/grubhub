using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Billing.GetBillingDetails
{
    public class GetBillingDetailsActionTests : ActionTestBase
    {
        public GetBillingDetailsActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthenticated()
        {
            var response = await factory.GetClient().Get(
                $"/restaurants/{Guid.NewGuid()}/billing");

            response.StatusCode.ShouldBe(401);
        }
    }
}
