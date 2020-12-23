using System;
using System.Threading.Tasks;
using Application.Restaurants;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;
using Web.Actions.Restaurants.UpdateOpeningTimes;
using Xunit;

namespace WebTests.Integration.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningTimesTests : WebIntegrationTestBase
    {
        public UpdateOpeningTimesTests(WebAppIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Opening_Times()
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

            var request = new UpdateOpeningTimesRequest()
            {
                MondayOpen = "10:30",
                MondayClose = "16:00",
            };

            var response = await Put($"/restaurants/{restaurant.Id.Value}/opening-times", request);
            Assert.Equal(200, (int)response.StatusCode);

            var restaurantDto = await Get<RestaurantDto>($"/restaurants/{restaurant.Id.Value}");
            Assert.NotNull(restaurantDto.OpeningTimes.Monday);
            Assert.Null(restaurantDto.OpeningTimes.Tuesday);
            Assert.Null(restaurantDto.OpeningTimes.Wednesday);
            Assert.Null(restaurantDto.OpeningTimes.Thursday);
            Assert.Null(restaurantDto.OpeningTimes.Friday);
            Assert.Null(restaurantDto.OpeningTimes.Saturday);
            Assert.Null(restaurantDto.OpeningTimes.Sunday);
            Assert.Equal("10:30", restaurantDto.OpeningTimes.Monday.Open);
            Assert.Equal("16:00", restaurantDto.OpeningTimes.Monday.Close);
        }
    }
}
