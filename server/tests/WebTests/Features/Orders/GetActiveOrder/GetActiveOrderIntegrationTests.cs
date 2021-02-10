using System;
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

        [Theory]
        [InlineData(Web.Domain.Orders.OrderStatus.Active)]
        [InlineData(Web.Domain.Orders.OrderStatus.Placed)]
        public async Task It_Returns_The_Authenticated_Users_Active_Order(
            Web.Domain.Orders.OrderStatus status)
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
                MenuItem = menuItem,
                MenuItemId = menuItem.Id,
            };

            var order = new Order()
            {
                Restaurant = restaurant,
                User = user,
                Status = status,
                Address = "2 Electric Avenue",
                PlacedAt = DateTime.UtcNow,
                Items = { orderItem },
            };

            fixture.Insert(restaurant, user, order);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get(
                $"/restaurants/{order.RestaurantId}/order");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto>();

            data.Id.ShouldBe(order.Id);
            data.RestaurantId.ShouldBe(order.RestaurantId);
            data.UserId.ShouldBe(order.UserId);
            data.Status.ShouldBe(status.ToString());
            data.Address.ShouldBe(order.Address);
            data.PlacedAt.Value.ShouldBe(order.PlacedAt.Value, TimeSpan.FromSeconds(0.000001));
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
            var restaurant = new Restaurant();

            var user = new User();

            fixture.Insert(restaurant, user);

            var response = await fixture.GetAuthenticatedClient(user.Id).Get(
                $"/restaurants/{restaurant.Id}/order");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto>();

            data.ShouldBeNull();
        }
    }
}
