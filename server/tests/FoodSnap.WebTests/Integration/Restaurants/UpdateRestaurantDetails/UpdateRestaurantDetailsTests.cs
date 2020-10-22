using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Restaurants;
using FoodSnap.Web.Actions.Restaurants.UpdateRestaurantDetails;
using Xunit;

namespace FoodSnap.WebTests.Integration.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsTests : WebTestBase
    {
        public UpdateRestaurantDetailsTests(WebAppTestFixture fixture) : base(fixture)
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
            };

            var response = await Put($"/restaurants/{restaurant.Id.Value}", request);

            Assert.Equal(200, (int)response.StatusCode);

            var restaurantDto = await Get<RestaurantDto>($"/restaurants/{restaurant.Id.Value}");
            Assert.Equal("Main Chow", restaurantDto.Name);
            Assert.Equal("09876543210", restaurantDto.PhoneNumber);
        }
    }
}
