using System;
using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdTests : StubbedWebTestBase
    {
        public GetRestaurantByIdTests(StubbedWebAppTestFixture fixture) : base(fixture)
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
