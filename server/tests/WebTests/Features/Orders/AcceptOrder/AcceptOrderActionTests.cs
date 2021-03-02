using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Orders.AcceptOrder
{
    public class AcceptOrderActionTests : ActionTestBase
    {
        public AcceptOrderActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await factory.GetClient().Put(
                $"/orders/{Guid.NewGuid()}/accept");

            response.StatusCode.ShouldBe(401);
        }
    }
}
