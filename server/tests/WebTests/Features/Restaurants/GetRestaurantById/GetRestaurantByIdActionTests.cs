using System;
using System.Threading.Tasks;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdActionTests : WebActionTestBase
    {
        public GetRestaurantByIdActionTests(WebActionTestFixture fixture) : base(fixture)
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
