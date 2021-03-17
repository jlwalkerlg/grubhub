using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.GetRestaurantById;
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
            var menu = restaurant.Menu;

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

            menu.Categories.Add(pizza);
            menu.Categories.Add(burgers);

            Insert(restaurant);

            var response = await factory.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantModel = await response.GetData<GetRestaurantByIdAction.RestaurantModel>();

            restaurantModel.Id.ShouldBe(restaurant.Id);
            restaurantModel.ManagerId.ShouldBe(restaurant.ManagerId);
            restaurantModel.Name.ShouldBe(restaurant.Name);
            restaurantModel.Description.ShouldBe(restaurant.Description);
            restaurantModel.PhoneNumber.ShouldBe(restaurant.PhoneNumber);
            restaurantModel.Status.ShouldBe(restaurant.Status);
            restaurantModel.AddressLine1.ShouldBe(restaurant.AddressLine1);
            restaurantModel.AddressLine2.ShouldBe(restaurant.AddressLine2);
            restaurantModel.City.ShouldBe(restaurant.City);
            restaurantModel.Postcode.ShouldBe(restaurant.Postcode);
            restaurantModel.Latitude.ShouldBe(restaurant.Latitude);
            restaurantModel.Longitude.ShouldBe(restaurant.Longitude);
            restaurantModel.OpeningTimes.Monday?.ShouldBe(restaurant.MondayOpen, restaurant.MondayClose);
            restaurantModel.OpeningTimes.Tuesday?.ShouldBe(restaurant.TuesdayOpen, restaurant.TuesdayClose);
            restaurantModel.OpeningTimes.Wednesday?.ShouldBe(restaurant.WednesdayOpen, restaurant.WednesdayClose);
            restaurantModel.OpeningTimes.Thursday?.ShouldBe(restaurant.ThursdayOpen, restaurant.ThursdayClose);
            restaurantModel.OpeningTimes.Friday?.ShouldBe(restaurant.FridayOpen, restaurant.FridayClose);
            restaurantModel.OpeningTimes.Saturday?.ShouldBe(restaurant.SaturdayOpen, restaurant.SaturdayClose);
            restaurantModel.OpeningTimes.Sunday?.ShouldBe(restaurant.SundayOpen, restaurant.SundayClose);
            restaurantModel.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            restaurantModel.MaxDeliveryDistanceInKm.ShouldBe(restaurant.MaxDeliveryDistanceInKm);
            restaurantModel.MinimumDeliverySpend.ShouldBe(restaurant.MinimumDeliverySpend);
            restaurantModel.EstimatedDeliveryTimeInMinutes.ShouldBe(restaurant.EstimatedDeliveryTimeInMinutes);
            restaurantModel.Menu.RestaurantId.ShouldBe(restaurant.Id);

            var categories = restaurantModel.Menu.Categories
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
            var restaurant = new Restaurant()
            {
                Menu = null,
            };

            Insert(restaurant);

            var response = await factory.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantModel = await response.GetData<GetRestaurantByIdAction.RestaurantModel>();

            restaurantModel.Id.ShouldBe(restaurant.Id);
            restaurantModel.Menu.ShouldBeNull();
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var response = await factory.GetClient().Get($"/restaurants/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(404);
        }
    }
}
