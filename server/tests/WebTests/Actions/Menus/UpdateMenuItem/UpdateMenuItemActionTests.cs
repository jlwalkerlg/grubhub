using System;
using System.Threading.Tasks;
using Web.Features.Menus.UpdateMenuItem;
using Xunit;

namespace WebTests.Actions.Menus.UpdateMenuItem
{
    public class UpdateMenuItemActionTests : WebActionTestBase
    {
        public UpdateMenuItemActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "Hawaiian",
                Description = "Ham & pineapple",
                Price = 11.99m,
            });

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita", new UpdateMenuItemRequest
            {
                NewItemName = "",
                Description = "",
                Price = -10m,
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("newItemName"));
            Assert.True(errors.ContainsKey("description"));
            Assert.True(errors.ContainsKey("price"));
        }
    }
}
