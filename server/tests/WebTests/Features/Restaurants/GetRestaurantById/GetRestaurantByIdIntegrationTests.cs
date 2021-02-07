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
            var restaurant = new Restaurant();

            var margherita = new MenuItem()
            {
                Name = "Margherita",
                Description = "Cheese & tomato",
            };

            var pizza = new MenuCategory()
            {
                Name = "Pizza",
                Items = new() { margherita },
            };

            var burgers = new MenuCategory()
            {
                Name = "Burgers",
                Items = new(),
            };

            var menu = new Menu()
            {
                Restaurant = restaurant,
                Categories = new() { pizza, burgers },
            };

            fixture.Insert(restaurant, menu);

            var items = fixture.UseTestDbContext(db => db.MenuItems.ToArray());

            var response = await fixture.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantDto = await response.GetData<RestaurantDto>();

            restaurantDto.Id.ShouldBe(restaurant.Id);
            restaurantDto.ManagerId.ShouldBe(restaurant.ManagerId);
            restaurantDto.Name.ShouldBe(restaurant.Name);
            restaurantDto.Description.ShouldBe(restaurant.Description);
            restaurantDto.PhoneNumber.ShouldBe(restaurant.PhoneNumber);
            restaurantDto.Status.ShouldBe(restaurant.Status);
            restaurantDto.Address.ShouldBe(restaurant.Address);
            restaurantDto.Latitude.ShouldBe(restaurant.Latitude);
            restaurantDto.Longitude.ShouldBe(restaurant.Longitude);
            restaurantDto.OpeningTimes.Monday.ShouldBe(restaurant.MondayOpen, restaurant.MondayClose);
            restaurantDto.OpeningTimes.Tuesday.ShouldBe(restaurant.TuesdayOpen, restaurant.TuesdayClose);
            restaurantDto.OpeningTimes.Wednesday.ShouldBe(restaurant.WednesdayOpen, restaurant.WednesdayClose);
            restaurantDto.OpeningTimes.Thursday.ShouldBe(restaurant.ThursdayOpen, restaurant.ThursdayClose);
            restaurantDto.OpeningTimes.Friday.ShouldBe(restaurant.FridayOpen, restaurant.FridayClose);
            restaurantDto.OpeningTimes.Saturday.ShouldBe(restaurant.SaturdayOpen, restaurant.SaturdayClose);
            restaurantDto.OpeningTimes.Sunday.ShouldBe(restaurant.SundayOpen, restaurant.SundayClose);
            restaurantDto.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            restaurantDto.MaxDeliveryDistanceInKm.ShouldBe(restaurant.MaxDeliveryDistanceInKm);
            restaurantDto.MinimumDeliverySpend.ShouldBe(restaurant.MinimumDeliverySpend);
            restaurantDto.EstimatedDeliveryTimeInMinutes.ShouldBe(restaurant.EstimatedDeliveryTimeInMinutes);
            restaurantDto.Menu.RestaurantId.ShouldBe(restaurant.Id);

            var categories = restaurantDto.Menu.Categories
                .OrderBy(x => x.Name)
                .ToArray();

            categories.Length.ShouldBe(2);

            categories[0].Name.ShouldBe(burgers.Name);
            categories[0].Items.ShouldBeEmpty();

            categories[1].Name.ShouldBe(pizza.Name);
            categories[1].Items.ShouldHaveSingleItem();
            categories[1].Items[0].Name.ShouldBe(margherita.Name);
            categories[1].Items[0].Description.ShouldBe(margherita.Description);
            categories[1].Items[0].Price.ShouldBe(margherita.Price);
        }

        [Fact]
        public async Task The_Menu_Is_Null_If_No_Menu_Exists()
        {
            var restaurant = new Restaurant();

            fixture.Insert(restaurant);

            var response = await fixture.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantDto = await response.GetData<RestaurantDto>();

            restaurantDto.Id.ShouldBe(restaurant.Id);
            restaurantDto.Menu.ShouldBeNull();
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var response = await fixture.GetClient().Get($"/restaurants/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(404);
        }
    }
}
