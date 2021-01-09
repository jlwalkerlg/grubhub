using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsActionTests : HttpTestBase
    {
        public SearchRestaurantsActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetClient().Get("/restaurants?postcode=MN121NM");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().Result.ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
