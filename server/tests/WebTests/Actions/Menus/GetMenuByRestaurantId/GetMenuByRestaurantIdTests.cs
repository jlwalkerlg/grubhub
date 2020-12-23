using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Actions.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdTests : WebActionTestBase
    {
        public GetMenuByRestaurantIdTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Get($"/restaurants/{Guid.NewGuid()}/menu");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
