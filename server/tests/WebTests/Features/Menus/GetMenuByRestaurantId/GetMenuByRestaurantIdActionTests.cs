using System;
using System.Threading.Tasks;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdActionTests : WebActionTestBase
    {
        public GetMenuByRestaurantIdActionTests(WebActionTestFixture fixture) : base(fixture)
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
