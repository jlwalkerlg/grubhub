using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateCuisines;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesIntegrationTests : IntegrationTestBase
    {
        public UpdateCuisinesIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Cuisines()
        {
            var italian = new Cuisine() { Name = "Italian" };
            var thai = new Cuisine() { Name = "Thai" };
            var indian = new Cuisine() { Name = "Indian" };

            var restaurant = new Restaurant()
            {
                Cuisines = new() { indian },
            };

            Insert(restaurant, italian, thai, indian);

            var request = new UpdateCuisinesRequest()
            {
                Cuisines = new() { italian.Name, thai.Name },
            };

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Put(
                $"/restaurants/{restaurant.Id}/cuisines",
                request);

            response.StatusCode.ShouldBe(200);

            var found = UseTestDbContext(db => db.RestaurantCuisines.ToList());

            found.Count.ShouldBe(2);
            found.ShouldContain(x => x.RestaurantId == restaurant.Id && x.CuisineName == italian.Name);
            found.ShouldContain(x => x.RestaurantId == restaurant.Id && x.CuisineName == thai.Name);
        }
    }
}
