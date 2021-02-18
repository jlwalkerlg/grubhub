using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsActionTests : ActionTestBase
    {
        public SearchRestaurantsActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await GetClient().Get("/restaurants?postcode=MN121NM");

            response.StatusCode.ShouldBe(400);
        }
    }
}
