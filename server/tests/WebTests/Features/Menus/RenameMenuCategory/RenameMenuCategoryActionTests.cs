using System;
using System.Threading.Tasks;
using Web.Features.Menus.RenameMenuCategory;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryActionTests : WebActionTestBase
    {
        public RenameMenuCategoryActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza", new RenameMenuCategoryRequest
            {
                NewName = "Curry",
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza", new RenameMenuCategoryRequest
            {
                NewName = "Curry",
            });

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var response = await Put($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza", new RenameMenuCategoryRequest
            {
                NewName = "",
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("newName"));
        }
    }
}
