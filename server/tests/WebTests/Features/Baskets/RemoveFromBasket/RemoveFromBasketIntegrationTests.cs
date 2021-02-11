using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketIntegrationTests : IntegrationTestBase
    {
        public RemoveFromBasketIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Removes_An_Item_From_The_Basket()
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
                MenuItemId = menuItem.Id,
                Quantity = 1,
            };

            var basket = new Basket()
            {
                User = user,
                Restaurant = restaurant,
                Items = { basketItem },
            };

            fixture.Insert(restaurant, user, basket);

            var response = await fixture.GetAuthenticatedClient(user.Id).Delete(
                $"/restaurants/{basket.RestaurantId}/basket/items/{menuItem.Id}");

            response.StatusCode.ShouldBe(204);

            var items = fixture.UseTestDbContext(db => db.BasketItems.ToArray());

            items.ShouldBeEmpty();
        }
    }
}
