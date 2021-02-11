using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Baskets;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Baskets.GetBasketByRestaurantId
{
    public class GetBasketByRestaurantIdIntegrationTests : IntegrationTestBase
    {
        public GetBasketByRestaurantIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Authenticated_Users_Basket()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var basketItem = new BasketItem()
            {
                MenuItem = menuItem,
                MenuItemId = menuItem.Id,
            };

            var basket = new Basket()
            {
                Restaurant = restaurant,
                User = user,
                Items = { basketItem },
            };

            fixture.Insert(restaurant, user, basket);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get(
                $"/restaurants/{basket.RestaurantId}/basket");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<BasketDto>();

            data.RestaurantId.ShouldBe(basket.RestaurantId);
            data.UserId.ShouldBe(basket.UserId);
            data.Items.ShouldHaveSingleItem();

            var item = data.Items.Single();

            item.MenuItemId.ShouldBe(menuItem.Id);
            item.MenuItemName.ShouldBe(menuItem.Name);
            item.MenuItemDescription.ShouldBe(menuItem.Description);
            item.MenuItemPrice.ShouldBe(menuItem.Price);
            item.Quantity.ShouldBe(1);
        }

        [Fact]
        public async Task It_Returns_Null_If_The_User_Doesnt_Have_A_Basket_For_The_Restaurant()
        {
            var restaurant = new Restaurant();

            var user = new User();

            fixture.Insert(restaurant, user);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get(
                $"/restaurants/{restaurant.Id}/basket");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<BasketDto>();

            data.ShouldBeNull();
        }
    }
}
