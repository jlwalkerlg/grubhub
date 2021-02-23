using System;
using System.Threading.Tasks;
using Web.Features.Baskets.UpdateBasketItemQuantity;
using Xunit;

namespace WebTests.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityActionTests : ActionTestBase
    {
        public UpdateBasketItemQuantityActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await factory.GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/basket/items/{Guid.NewGuid()}",
                new UpdateBasketItemQuantityRequest()
                {
                    Quantity = 1,
                });
            
            response.StatusCode.ShouldBe(401);
        }
    }
}