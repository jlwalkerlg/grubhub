using System;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using Xunit;

namespace WebTests.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdIntegrationTests : WebIntegrationTestBase
    {
        public GetRestaurantByIdIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
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
            restaurant.OpeningTimes = new OpeningTimes()
            {
                Monday = new OpeningHours(new TimeSpan(10, 30, 0), new TimeSpan(16, 0, 0)),
            };
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.DeliveryFee = new Money(1.5m);
            restaurant.EstimatedDeliveryTimeInMinutes = 40;

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
            Assert.Equal("10:30", restaurantDto.OpeningTimes.Monday.Open);
            Assert.Equal("16:00", restaurantDto.OpeningTimes.Monday.Close);
            Assert.Equal(1.5m, restaurantDto.DeliveryFee);
            Assert.Equal(10m, restaurantDto.MinimumDeliverySpend);
            Assert.Equal(40, restaurantDto.EstimatedDeliveryTimeInMinutes);
        }
    }
}
