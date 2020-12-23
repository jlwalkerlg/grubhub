using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Actions.Menus.RemoveMenuItem
{
    public class RemoveMenuItemTests : WebActionTestBase
    {
        public RemoveMenuItemTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var response = await Delete($"/restaurants/{Guid.NewGuid()}/menu/categories/Pizza/items/Margherita");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
