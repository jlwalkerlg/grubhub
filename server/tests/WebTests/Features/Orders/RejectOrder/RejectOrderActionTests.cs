using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Orders.RejectOrder
{
    public class RejectOrderActionTests : ActionTestBase
    {
        public RejectOrderActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Required_Authentication()
        {
            var response = await factory.GetClient().Put(
                $"/orders/{Guid.NewGuid()}/reject");

            response.StatusCode.ShouldBe(401);
        }
    }
}
