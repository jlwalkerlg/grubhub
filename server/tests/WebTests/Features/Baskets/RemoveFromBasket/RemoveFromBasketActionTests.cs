using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketActionTests : HttpTestBase
    {
        public RemoveFromBasketActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/basket/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Delete(
                $"/restaurants/{Guid.NewGuid()}/basket/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}