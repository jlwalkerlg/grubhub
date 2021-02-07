using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.AddToOrder;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.AddToOrder
{
    public class AddToOrderIntegrationTests : IntegrationTestBase
    {
        public AddToOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Starts_A_New_Order()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            fixture.Insert(restaurant, user);

            var request = new AddToOrderRequest()
            {
                MenuItemId = menuItem.Id,
                Quantity = 1,
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Post(
                $"/order/{menu.RestaurantId}",
                request);

            response.StatusCode.ShouldBe(200);

            var order = fixture.UseTestDbContext(db => db.Orders.Single());

            order.UserId.ShouldBe(user.Id);
            order.RestaurantId.ShouldBe(menu.RestaurantId);

            var orderItem = fixture.UseTestDbContext(db => db.OrderItems.Single());

            orderItem.OrderId.ShouldBe(order.Id);
            orderItem.MenuItemId.ShouldBe(request.MenuItemId);
            orderItem.Quantity.ShouldBe(request.Quantity);
        }

        [Fact]
        public async Task It_Adds_To_An_Existing_Order()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var order = new Order()
            {
                Restaurant = restaurant,
                User = user,
            };

            fixture.Insert(restaurant, user, order);

            var request = new AddToOrderRequest()
            {
                MenuItemId = menuItem.Id,
                Quantity = 3,
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Post(
                $"/order/{order.RestaurantId}",
                request);

            response.StatusCode.ShouldBe(200);

            var orderItem = fixture.UseTestDbContext(db => db.OrderItems.Single());

            orderItem.OrderId.ShouldBe(order.Id);
            orderItem.MenuItemId.ShouldBe(request.MenuItemId);
            orderItem.Quantity.ShouldBe(request.Quantity);
        }
    }
}
