using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketActionTests : ActionTestBase
    {
        public RemoveFromBasketActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await GetClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/basket/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await GetAuthenticatedClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/basket/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(400);
        }
    }
}
