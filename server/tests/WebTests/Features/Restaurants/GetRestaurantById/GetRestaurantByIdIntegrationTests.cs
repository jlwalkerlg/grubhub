using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdIntegrationTests : IntegrationTestBase
    {
        public GetRestaurantByIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Restaurant()
        {
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var margherita = new MenuItem()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m,
            };

            var pizza = new MenuCategory()
            {
                Name = "Pizza",
                Items = new() { margherita },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = new() { pizza },
            };

            fixture.Insert(manager, restaurant, menu);

            var items = fixture.UseTestDbContext(db => db.MenuItems.ToArray());

            var response = await fixture.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantDto = await response.GetData<RestaurantDto>();

            restaurantDto.Id.ShouldBe(restaurant.Id);
            restaurantDto.ManagerId.ShouldBe(manager.Id);
            restaurantDto.Name.ShouldBe(restaurant.Name);
            restaurantDto.PhoneNumber.ShouldBe(restaurant.PhoneNumber);
            restaurantDto.Status.ShouldBe(restaurant.Status);
            restaurantDto.Address.ShouldBe(restaurant.Address);
            restaurantDto.Latitude.ShouldBe(restaurant.Latitude);
            restaurantDto.Longitude.ShouldBe(restaurant.Longitude);
            restaurantDto.OpeningTimes.Monday.ShouldBeNull();
            restaurantDto.OpeningTimes.Tuesday.ShouldBeNull();
            restaurantDto.OpeningTimes.Wednesday.ShouldBeNull();
            restaurantDto.OpeningTimes.Thursday.ShouldBeNull();
            restaurantDto.OpeningTimes.Friday.ShouldBeNull();
            restaurantDto.OpeningTimes.Saturday.ShouldBeNull();
            restaurantDto.OpeningTimes.Sunday.ShouldBeNull();
            restaurantDto.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            restaurantDto.MaxDeliveryDistanceInKm.ShouldBe(restaurant.MaxDeliveryDistanceInKm);
            restaurantDto.MinimumDeliverySpend.ShouldBe(restaurant.MinimumDeliverySpend);
            restaurantDto.EstimatedDeliveryTimeInMinutes.ShouldBe(restaurant.EstimatedDeliveryTimeInMinutes);
            restaurantDto.Menu.RestaurantId.ShouldBe(restaurant.Id);
            restaurantDto.Menu.Categories.ShouldHaveSingleItem();
            restaurantDto.Menu.Categories[0].Name.ShouldBe(pizza.Name);
            restaurantDto.Menu.Categories[0].Items.ShouldHaveSingleItem();
            restaurantDto.Menu.Categories[0].Items[0].Name.ShouldBe(margherita.Name);
            restaurantDto.Menu.Categories[0].Items[0].Description.ShouldBe(margherita.Description);
            restaurantDto.Menu.Categories[0].Items[0].Price.ShouldBe(margherita.Price);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var response = await fixture.GetClient().Get($"/restaurants/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(404);
        }
    }
}
