using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Actions.Restaurants;
using Xunit;

namespace FoodSnap.WebTests.Integration.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdTests : WebIntegrationTestBase
    {
        public GetRestaurantByIdTests(WebAppIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Restaurant()
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

            var restaurantDto = await Get<RestaurantDto>($"/restaurants/{restaurant.Id.Value}");
            Assert.Equal(restaurant.Id.Value, restaurantDto.Id);
            Assert.Equal(manager.Id.Value, restaurantDto.ManagerId);
            Assert.Equal("Chow Main", restaurantDto.Name);
            Assert.Equal("01234567890", restaurantDto.PhoneNumber);
            Assert.Equal(RestaurantStatus.PendingApproval.ToString(), restaurantDto.Status);
            Assert.Equal("12 Maine Road, Madchester, MN12 1NM", restaurantDto.Address);
            Assert.Equal(1, restaurantDto.Latitude);
            Assert.Equal(2, restaurantDto.Longitude);
        }
    }
}
