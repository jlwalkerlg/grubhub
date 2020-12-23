using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdTests : WebActionTestBase
    {
        public GetRestaurantByIdTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Get($"/restaurants/{Guid.NewGuid()}");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
