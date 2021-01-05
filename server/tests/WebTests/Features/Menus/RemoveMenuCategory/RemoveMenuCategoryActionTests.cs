using System;
using System.Threading.Tasks;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryActionTests : WebActionTestBase
    {
        public RemoveMenuCategoryActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza");

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
