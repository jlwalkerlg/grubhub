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
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            fixture.Insert(manager, restaurant);

            var request = new UpdateRestaurantDetailsRequest()
            {
                Name = "Main Chow",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 15,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await fixture.GetAuthenticatedClient(manager.Id).Put(
                $"/restaurants/{restaurant.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.Restaurants.Single());

            found.Name.ShouldBe(request.Name);
            found.PhoneNumber.ShouldBe(request.PhoneNumber);
            found.MinimumDeliverySpend.ShouldBe(request.MinimumDeliverySpend);
            found.DeliveryFee.ShouldBe(request.DeliveryFee);
            found.MaxDeliveryDistanceInKm.ShouldBe(request.MaxDeliveryDistanceInKm);
            found.EstimatedDeliveryTimeInMinutes.ShouldBe(request.EstimatedDeliveryTimeInMinutes);
        }
    }
}
