using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Baskets.AddToBasket;
using Xunit;

namespace WebTests.Features.Baskets.AddToBasket
{
    public class AddToBasketActionTests : ActionTestBase
    {
        public AddToBasketActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new AddToBasketRequest();

            var response = await GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/basket",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new AddToBasketRequest()
            {
                MenuItemId = Guid.Empty,
                Quantity = 0,
            };

            var response = await GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.Empty}/basket",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("menuItemId");
            errors.ShouldContainKey("quantity");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new AddToBasketRequest()
            {
                MenuItemId = Guid.NewGuid(),
                Quantity = 1,
            };

            var response = await GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/basket",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
