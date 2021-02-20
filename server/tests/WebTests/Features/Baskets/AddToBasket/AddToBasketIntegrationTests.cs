using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Web.Features.Baskets.AddToBasket;
using WebTests.TestData;
using Xunit;
using Xunit.Abstractions;

namespace WebTests.Features.Baskets.AddToBasket
{
    public class AddToBasketIntegrationTests : IntegrationTestBase
    {
        private readonly ITestOutputHelper output;

        public AddToBasketIntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            this.output = output;
        }

        [Fact]
        public void Test() { }

        [Fact]
        public async Task It_Creates_A_New_Basket()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            Insert(restaurant, user);

            var request = new AddToBasketRequest()
            {
                MenuItemId = menuItem.Id,
                Quantity = 1,
            };

            var response = await factory.GetAuthenticatedClient(user.Id).Post(
                $"/restaurants/{menu.RestaurantId}/basket",
                request);

            response.StatusCode.ShouldBe(200);

            var basket = UseTestDbContext(db => db.Baskets.Single());

            basket.UserId.ShouldBe(user.Id);
            basket.RestaurantId.ShouldBe(menu.RestaurantId);

            var basketItem = UseTestDbContext(db => db.BasketItems.Single());

            basketItem.MenuItemId.ShouldBe(request.MenuItemId);
            basketItem.Quantity.ShouldBe(request.Quantity);
        }

        [Fact]
        public async Task It_Adds_To_An_Existing_Basket()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var basket = new Basket()
            {
                Restaurant = restaurant,
                User = user,
            };

            Insert(restaurant, user, basket);

            var request = new AddToBasketRequest()
            {
                MenuItemId = menuItem.Id,
                Quantity = 3,
            };

            var response = await factory.GetAuthenticatedClient(user.Id).Post(
                $"/restaurants/{basket.RestaurantId}/basket",
                request);

            response.StatusCode.ShouldBe(200);

            var basketItem = UseTestDbContext(db => db.BasketItems.Single());

            basketItem.MenuItemId.ShouldBe(request.MenuItemId);
            basketItem.Quantity.ShouldBe(request.Quantity);
        }
    }
}
