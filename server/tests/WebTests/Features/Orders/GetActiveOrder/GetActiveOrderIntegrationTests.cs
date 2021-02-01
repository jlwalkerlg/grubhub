using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.GetActiveOrder;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.GetActiveOrder
{
    public class GetActiveOrderIntegrationTests : IntegrationTestBase
    {
        public GetActiveOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Authenticated_Users_Order()
        {
            var menuItem = new MenuItem();

            var menuCategory = new MenuCategory()
            {
                Items = { menuItem },
            };

            var menu = new Menu()
            {
                Categories = { menuCategory },
            };

            var user = new User();

            var orderItem = new OrderItem()
            {
                MenuItem = menuItem,
                MenuItemId = menuItem.Id,
            };

            var order = new Order()
            {
                Restaurant = menu.Restaurant,
                User = user,
                Items = { orderItem },
            };

            fixture.Insert(menu, user, order);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get($"/order/{order.RestaurantId}");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto>();

            data.Id.ShouldBe(order.Id);
            data.RestaurantId.ShouldBe(order.RestaurantId);
            data.UserId.ShouldBe(order.UserId);
            data.Status.ShouldBe("Active");
            data.Items.ShouldHaveSingleItem();

            var item = data.Items.Single();

            item.MenuItemId.ShouldBe(menuItem.Id);
            item.MenuItemName.ShouldBe(menuItem.Name);
            item.MenuItemDescription.ShouldBe(menuItem.Description);
            item.MenuItemPrice.ShouldBe(menuItem.Price);
            item.Quantity.ShouldBe(1);
        }

        [Fact]
        public async Task It_Returns_Null_If_The_User_Doesnt_Have_An_Active_Order_At_The_Restaurant()
        {
            var menu = new Menu();

            var user = new User();

            fixture.Insert(menu, user);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get($"/order/{menu.RestaurantId}");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto>();

            data.ShouldBeNull();
        }
    }
}
