using System;
using System.Threading.Tasks;
using Web.Actions.Menus.AddMenuItem;
using Xunit;

namespace WebTests.Actions.Menus.AddMenuItem
{
    public class AddMenuItemTests : WebActionTestBase
    {
        public AddMenuItemTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 10m,
            });

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/items", new AddMenuItemRequest
            {
                CategoryName = "",
                ItemName = "",
                Description = "",
                Price = -10m,
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("categoryName"));
            Assert.True(errors.ContainsKey("itemName"));
            Assert.True(errors.ContainsKey("description"));
            Assert.True(errors.ContainsKey("price"));
        }
    }
}
