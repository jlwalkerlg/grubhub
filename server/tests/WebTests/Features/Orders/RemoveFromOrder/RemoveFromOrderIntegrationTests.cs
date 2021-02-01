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
            var margherita = new MenuItem();

            var pizza = new MenuCategory()
            {
                Items = { margherita },
            };

            var menu = new Menu()
            {
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
                User = user,
                Restaurant = menu.Restaurant,
                Items = { orderItem },
            };

            fixture.Insert(menu, user, order);

            var response = await fixture.GetAuthenticatedClient(user.Id).Delete(
                $"/order/{order.RestaurantId}/items/{margherita.Id}");

            response.StatusCode.ShouldBe(204);

            var items = fixture.UseTestDbContext(db => db.OrderItems.ToArray());

            items.ShouldBeEmpty();
        }
    }
}
