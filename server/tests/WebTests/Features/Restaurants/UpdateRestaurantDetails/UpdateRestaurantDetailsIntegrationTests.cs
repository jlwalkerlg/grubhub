using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsIntegrationTests : IntegrationTestBase
    {
        public UpdateRestaurantDetailsIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Restaurants_Details()
        {
            var restaurant = new Restaurant();

            Insert(restaurant);

            var request = new UpdateRestaurantDetailsRequest()
            {
                Name = "Main Chow",
                Description = "Dog Days Are Over",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 15,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await factory.GetAuthenticatedClient(restaurant.ManagerId).Put(
                $"/restaurants/{restaurant.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = UseTestDbContext(db => db.Restaurants.Single());

            found.Name.ShouldBe(request.Name);
            found.Description.ShouldBe(request.Description);
            found.PhoneNumber.ShouldBe(request.PhoneNumber);
            found.MinimumDeliverySpend.ShouldBe(request.MinimumDeliverySpend);
            found.DeliveryFee.ShouldBe(request.DeliveryFee);
            found.MaxDeliveryDistanceInKm.ShouldBe(request.MaxDeliveryDistanceInKm);
            found.EstimatedDeliveryTimeInMinutes.ShouldBe(
                request.EstimatedDeliveryTimeInMinutes);
        }
    }
}
