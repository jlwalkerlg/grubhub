using System.Threading.Tasks;
using Web.Features.Orders.AddToOrder;
using Xunit;

namespace WebTests.Features.Orders.GetActiveOrder
{
    public class GetActiveOrderActionTests : HttpTestBase
    {
        public GetActiveOrderActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var command = new AddToOrderCommand();

            var response = await fixture.GetClient().Get("/order");

            response.StatusCode.ShouldBe(401);
        }
    }
}
