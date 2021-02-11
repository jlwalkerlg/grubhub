using System;
using System.Threading.Tasks;
using Web.Features.Baskets.AddToBasket;
using Xunit;

namespace WebTests.Features.Baskets.GetBasketByRestaurantId
{
    public class GetBasketByRestaurantIdActionTests : HttpTestBase
    {
        public GetBasketByRestaurantIdActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Get(
                $"/restaurants/{Guid.NewGuid()}/basket");

            response.StatusCode.ShouldBe(401);
        }
    }
}
