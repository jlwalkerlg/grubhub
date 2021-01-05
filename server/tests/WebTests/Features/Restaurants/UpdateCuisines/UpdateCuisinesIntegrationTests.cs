using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants;
using Web.Features.Restaurants.UpdateCuisines;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesIntegrationTests : WebIntegrationTestBase
    {
        public UpdateCuisinesIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Cuisines()
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

            var pizza = new Cuisine("Pizza");
            var thai = new Cuisine("Thai");

            restaurant.SetCuisines(pizza);

            await fixture.InsertDb(manager, restaurant, menu, pizza, thai);

            await Login(manager);

            var request = new UpdateCuisinesRequest()
            {
                Cuisines = new() { "Pizza", "Thai" },
            };

            var response = await Put($"/restaurants/{restaurant.Id.Value}/cuisines", request);
            Assert.Equal(200, (int)response.StatusCode);

            var restaurantDto = await Get<RestaurantDto>($"/restaurants/{restaurant.Id.Value}");
            Assert.True(restaurant.IsEqual(restaurantDto));
        }
    }
}
