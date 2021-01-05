using System;
using System.Threading.Tasks;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsActionTests : WebActionTestBase
    {
        public UpdateRestaurantDetailsActionTests(WebActionTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new UpdateRestaurantDetailsRequest
            {
                Name = "Main Chow",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await Put($"/restaurants/{Guid.NewGuid()}", request);

            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            await Login();

            var request = new UpdateRestaurantDetailsRequest
            {
                Name = "Main Chow",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 10,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await Put($"/restaurants/{Guid.NewGuid()}", request);

            Assert.Equal(FailMiddlewareStub.Message, await response.GetErrorMessage());
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            await Login();

            var request = new UpdateRestaurantDetailsRequest
            {
                Name = "",
                PhoneNumber = "",
                MinimumDeliverySpend = -1m,
                DeliveryFee = -1m,
                MaxDeliveryDistanceInKm = -10,
                EstimatedDeliveryTimeInMinutes = -40,
            };

            var response = await Put($"/restaurants/{Guid.NewGuid()}", request);

            Assert.Equal(422, (int)response.StatusCode);

            var errors = await response.GetErrors();
            Assert.True(errors.ContainsKey("name"));
            Assert.True(errors.ContainsKey("phoneNumber"));
            Assert.True(errors.ContainsKey("minimumDeliverySpend"));
            Assert.True(errors.ContainsKey("deliveryFee"));
            Assert.True(errors.ContainsKey("maxDeliveryDistanceInKm"));
            Assert.True(errors.ContainsKey("estimatedDeliveryTimeInMinutes"));
        }
    }
}
