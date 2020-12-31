using System.Threading.Tasks;
using Xunit;

namespace WebTests.Actions.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsTests : WebActionTestBase
    {
        public SearchRestaurantsTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await Get("/restaurants?postcode=MN121NM");

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }
    }
}
