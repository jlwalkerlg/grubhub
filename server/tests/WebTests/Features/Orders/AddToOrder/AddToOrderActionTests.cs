using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.AddToOrder;
using Xunit;

namespace WebTests.Features.Orders.AddToOrder
{
    public class AddToOrderActionTests : HttpTestBase
    {
        public AddToOrderActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var command = new AddToOrderCommand();

            var response = await fixture.GetClient().Post(
                "/order",
                command);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var command = new AddToOrderCommand()
            {
                RestaurantId = Guid.Empty,
                MenuItemId = Guid.Empty,
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                "/order",
                command);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("menuItemId");
            errors.ShouldContainKey("restaurantId");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var command = new AddToOrderCommand()
            {
                RestaurantId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid(),
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                "/order",
                command);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
