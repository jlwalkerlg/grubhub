using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemActionTests : HttpTestBase
    {
        public UpdateMenuItemActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateMenuItemRequest()
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await fixture.GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateMenuItemRequest()
            {
                NewItemName = "",
                Description = "",
                Price = -1m,
            };

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("newItemName");
            errors.ShouldContainKey("description");
            errors.ShouldContainKey("price");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateMenuItemRequest()
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            };

            var response = await fixture.GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
