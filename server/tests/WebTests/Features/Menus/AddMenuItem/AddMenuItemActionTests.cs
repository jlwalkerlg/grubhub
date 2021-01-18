using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuItem;
using Xunit;

namespace WebTests.Features.Menus.AddMenuItem
{
    public class AddMenuItemActionTests : HttpTestBase
    {
        public AddMenuItemActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new AddMenuItemRequest()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            };

            var response = await fixture.GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new AddMenuItemRequest()
            {
                Name = "",
                Description = new string('c', 281),
                Price = -10m,
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items",
                request);

            var errors = response.GetErrors();

            errors.ShouldContainKey("name");
            errors.ShouldContainKey("description");
            errors.ShouldContainKey("price");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new AddMenuItemRequest()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/{Guid.NewGuid()}/items",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);

            response.StatusCode.ShouldBe(400);
        }
    }
}
