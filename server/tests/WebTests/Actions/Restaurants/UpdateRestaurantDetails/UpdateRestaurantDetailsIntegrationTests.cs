using System;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using Web.Actions.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace WebTests.Actions.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsIntegrationTests : WebIntegrationTestBase
    {
        public UpdateRestaurantDetailsIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Restaurants_Details()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            var menu = new Menu(restaurant.Id);

            await fixture.InsertDb(manager, restaurant, menu);
            await Login(manager);

            var request = new UpdateRestaurantDetailsRequest
            {
                Name = "Main Chow",
                PhoneNumber = "09876543210",
                MinimumDeliverySpend = 13m,
                DeliveryFee = 3m,
                MaxDeliveryDistanceInKm = 15,
                EstimatedDeliveryTimeInMinutes = 40,
            };

            var response = await Put($"/restaurants/{restaurant.Id.Value}", request);

            Assert.Equal(200, (int)response.StatusCode);

            var restaurantDto = await Get<RestaurantDto>($"/restaurants/{restaurant.Id.Value}");
            Assert.Equal("Main Chow", restaurantDto.Name);
            Assert.Equal("09876543210", restaurantDto.PhoneNumber);
            Assert.Equal(13m, restaurantDto.MinimumDeliverySpend);
            Assert.Equal(3m, restaurantDto.DeliveryFee);
            Assert.Equal(15, restaurantDto.MaxDeliveryDistanceInKm);
            Assert.Equal(40, restaurantDto.EstimatedDeliveryTimeInMinutes);
        }
    }
}
