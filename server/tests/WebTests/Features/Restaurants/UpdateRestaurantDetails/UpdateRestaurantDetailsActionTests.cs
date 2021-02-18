using Shouldly;
using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsActionTests : ActionTestBase
    {
        public UpdateRestaurantDetailsActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateRestaurantDetailsRequest();

            var response = await GetClient().Put(
                $"/restaurants/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new UpdateRestaurantDetailsRequest()
            {
                Name = "",
                Description = new string('c', 401),
                PhoneNumber = "",
                MinimumDeliverySpend = -1m,
                DeliveryFee = -1m,
                MaxDeliveryDistanceInKm = -10,
                EstimatedDeliveryTimeInMinutes = -40,
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("name");
            errors.ShouldContainKey("description");
            errors.ShouldContainKey("phoneNumber");
            errors.ShouldContainKey("minimumDeliverySpend");
            errors.ShouldContainKey("deliveryFee");
            errors.ShouldContainKey("maxDeliveryDistanceInKm");
            errors.ShouldContainKey("estimatedDeliveryTimeInMinutes");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new UpdateRestaurantDetailsRequest()
            {
                Name = "Main Chow",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await GetAuthenticatedClient().Put(
                $"/restaurants/{Guid.NewGuid()}",
                request);

            response.StatusCode.ShouldBe(400);
        }
    }
}
