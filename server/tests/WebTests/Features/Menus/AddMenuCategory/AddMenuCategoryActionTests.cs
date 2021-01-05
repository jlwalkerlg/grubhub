using System;
using System.Threading.Tasks;
using Web.Features.Menus.AddMenuCategory;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryActionTests : WebActionTestBase
    {
        public AddMenuCategoryActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "Pizza",
            });

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "Pizza",
            });

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var response = await Post($"/restaurants/{Guid.NewGuid()}/menu/categories", new AddMenuCategoryRequest
            {
                Name = "",
            });

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("name"));
        }
    }
}
