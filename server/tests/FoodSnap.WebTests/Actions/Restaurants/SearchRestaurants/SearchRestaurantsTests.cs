using System.Threading.Tasks;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsTests : WebActionTestBase
    {
        public SearchRestaurantsTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Get("/restaurants?postcode=MN121NM&maxDistanceInKm=10");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
