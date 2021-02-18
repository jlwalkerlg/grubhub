using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Baskets.GetBasketByRestaurantId
{
    public class GetBasketByRestaurantIdActionTests : ActionTestBase
    {
        public GetBasketByRestaurantIdActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await GetClient().Get(
                $"/restaurants/{Guid.NewGuid()}/basket");

            response.StatusCode.ShouldBe(401);
        }
    }
}
