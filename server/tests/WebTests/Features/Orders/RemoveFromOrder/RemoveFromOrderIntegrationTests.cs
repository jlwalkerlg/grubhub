using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.RemoveFromOrder
{
    public class RemoveFromOrderIntegrationTests : IntegrationTestBase
    {
        public RemoveFromOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Removes_An_Item_From_The_Order()
        {
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            var margherita = new MenuItem() { Name = "Margherita" };

            var pizza = new MenuCategory()
            {
                Name = "Pizza",
                Items = { margherita },
            };

            var menu = new Menu()
            {
                RestaurantId = restaurant.Id,
                Categories = { pizza },
            };

            var user = new User();

            var orderItem = new OrderItem()
            {
                MenuItemId = margherita.Id,
                Quantity = 1,
            };

            var order = new Order()
            {
                UserId = user.Id,
                RestaurantId = restaurant.Id,
                Items = { orderItem },
            };

            fixture.Insert(manager, restaurant, menu, user, order);

            var response = await fixture.GetAuthenticatedClient(user.Id).Delete(
                $"/order/items/{margherita.Id}");

            response.StatusCode.ShouldBe(204);

            var items = fixture.UseTestDbContext(db => db.OrderItems.ToArray());

            items.ShouldBeEmpty();
        }
    }
}
