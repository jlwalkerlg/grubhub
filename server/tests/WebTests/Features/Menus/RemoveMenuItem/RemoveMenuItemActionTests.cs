using System;
using System.Threading.Tasks;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemActionTests : WebActionTestBase
    {
        public RemoveMenuItemActionTests(WebActionTestFixture fixture) : base(fixture)
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
