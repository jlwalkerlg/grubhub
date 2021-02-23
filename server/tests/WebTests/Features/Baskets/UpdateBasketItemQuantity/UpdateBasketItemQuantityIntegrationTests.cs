using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Baskets.UpdateBasketItemQuantity;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityIntegrationTests : IntegrationTestBase
    {
        public UpdateBasketItemQuantityIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Updates_The_Items_Quantity()
        {
            var restaurant = new Restaurant();

            var menu = restaurant.Menu;
            var menuCategory = new MenuCategory();
            var menuItem = new MenuItem();
            menu.Categories.Add(menuCategory);
            menuCategory.Items.Add(menuItem);

            var basket = new Basket
            {
                Restaurant = restaurant,
            };

            var basketItem = new BasketItem()
            {
                MenuItem = menuItem,
                Quantity = 1,
            };
            basket.Items.Add(basketItem);
            
            Insert(restaurant, basket);

            var request = new UpdateBasketItemQuantityRequest()
            {
                Quantity = 2,
            };

            var response = await factory.GetAuthenticatedClient(basket.UserId).Put(
                $"/restaurants/{restaurant.Id}/basket/items/{menuItem.Id}",
                request);
            
            response.StatusCode.ShouldBe(200);
            
            var found = UseTestDbContext(db => db.BasketItems.Single());

            found.Quantity.ShouldBe(request.Quantity);
        }
    }
}