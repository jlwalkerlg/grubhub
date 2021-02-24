using System.Threading.Tasks;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Features.Orders.GetRestaurantOrders
{
    public class GetRestaurantOrdersActionTests : ActionTestBase
    {
        public GetRestaurantOrdersActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await factory.GetClient().Get(
                "/restaurant/orders");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Unauthorized_If_The_User_Is_Not_A_Manager()
        {
            var response = await factory.GetAuthenticatedClient(UserRole.Customer).Get(
                "/restaurant/orders");

            response.StatusCode.ShouldBe(403);
        }
    }
}
