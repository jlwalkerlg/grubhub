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
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var menuItem = new MenuItem();

            var menuCategory = new MenuCategory()
            {
                Items = { menuItem },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = { menuCategory },
            };

            var user = new User();

            fixture.Insert(manager, restaurant, menu, user);

            var request = new AddToOrderRequest()
            {
                MenuItemId = menuItem.Id,
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Post(
                $"/order/{restaurant.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var order = fixture.UseTestDbContext(db => db.Orders.Single());

            order.UserId.ShouldBe(user.Id);
            order.RestaurantId.ShouldBe(restaurant.Id);

            var orderItem = fixture.UseTestDbContext(db => db.OrderItems.Single());

            orderItem.OrderId.ShouldBe(order.Id);
            orderItem.MenuItemId.ShouldBe(request.MenuItemId);
            orderItem.Quantity.ShouldBe(1);
        }

        [Fact]
        public async Task It_Adds_To_An_Existing_Order()
        {
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var menuItem = new MenuItem();

            var menuCategory = new MenuCategory()
            {
                Items = { menuItem },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = { menuCategory },
            };

            var user = new User();

            var order = new Order()
            {
                UserId = user.Id,
                RestaurantId = restaurant.Id,
            };

            fixture.Insert(manager, restaurant, menu, user, order);

            var request = new AddToOrderRequest()
            {
                MenuItemId = menuItem.Id,
            };

            var response = await fixture.GetAuthenticatedClient(user.Id).Post(
                $"/order/{restaurant.Id}",
                request);

            response.StatusCode.ShouldBe(200);

            var orderItem = fixture.UseTestDbContext(db => db.OrderItems.Single());

            orderItem.OrderId.ShouldBe(order.Id);
            orderItem.MenuItemId.ShouldBe(request.MenuItemId);
            orderItem.Quantity.ShouldBe(1);
        }
    }
}
