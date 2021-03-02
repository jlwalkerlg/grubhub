using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Orders.DeliverOrder
{
    public class DeliverOrderActionTests : ActionTestBase
    {
        public DeliverOrderActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await factory.GetClient().Put(
                $"/orders/{Guid.NewGuid()}/deliver");

            response.StatusCode.ShouldBe(401);
        }
    }
}
