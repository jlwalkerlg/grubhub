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
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();

            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var orderItem = new OrderItem()
            {
                MenuItemId = menuItem.Id,
                Quantity = 1,
            };

            var order = new Order()
            {
                User = user,
                Restaurant = restaurant,
                Items = { orderItem },
            };

            fixture.Insert(restaurant, user, order);

            var response = await fixture.GetAuthenticatedClient(user.Id).Delete(
                $"/order/{order.RestaurantId}/items/{menuItem.Id}");

            response.StatusCode.ShouldBe(204);

            var items = fixture.UseTestDbContext(db => db.OrderItems.ToArray());

            items.ShouldBeEmpty();
        }
    }
}
