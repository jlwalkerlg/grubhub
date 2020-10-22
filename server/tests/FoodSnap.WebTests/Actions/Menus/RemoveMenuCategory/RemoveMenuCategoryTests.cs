using System;
using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryTests : StubbedWebTestBase
    {
        public RemoveMenuCategoryTests(StubbedWebAppTestFixture fixture) : base(fixture)
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
