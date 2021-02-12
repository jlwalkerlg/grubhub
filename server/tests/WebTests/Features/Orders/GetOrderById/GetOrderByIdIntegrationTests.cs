using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.GetActiveOrder;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.GetOrderById
{
    public class GetOrderByIdIntegrationTests : IntegrationTestBase
    {
        public GetOrderByIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthenticated()
        {
            var response = await fixture.GetClient().Get(
                $"/orders/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var response = await fixture.GetAuthenticatedClient().Get(
                $"/orders/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(404);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var order = new Order();

            fixture.Insert(order);

            var response = await fixture.GetAuthenticatedClient(Guid.NewGuid()).Get(
                $"/orders/{order.Id}");

            response.StatusCode.ShouldBe(403);
        }

        [Fact]
        public async Task It_Gets_The_Order()
        {
            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();
            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var order = new Order();
            order.Restaurant = restaurant;
            var orderItem = new OrderItem();
            orderItem.MenuItemId = menuItem.Id;
            orderItem.Quantity = 1;
            order.Items.Add(orderItem);

            fixture.Insert(restaurant, order);

            var response = await fixture.GetAuthenticatedClient(order.UserId).Get(
                $"/orders/{order.Id}");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto>();

            data.Id.ShouldBe(order.Id);
            data.UserId.ShouldBe(order.UserId);
            data.RestaurantId.ShouldBe(order.RestaurantId);
            data.Subtotal.ShouldBe(order.Subtotal);
            data.DeliveryFee.ShouldBe(order.DeliveryFee);
            data.ServiceFee.ShouldBe(order.ServiceFee);
            data.Status.ShouldBe(order.Status);
            data.Address.ShouldBe(order.Address);
            data.PlacedAt.ShouldBe(order.PlacedAt);
            data.RestaurantAddress.ShouldBe(restaurant.Address);
            data.PaymentIntentClientSecret.ShouldBe(order.PaymentIntentClientSecret);
            data.Items.ShouldHaveSingleItem();

            var item = data.Items.Single();

            item.MenuItemId.ShouldBe(menuItem.Id);
            item.MenuItemName.ShouldBe(menuItem.Name);
            item.MenuItemPrice.ShouldBe(menuItem.Price);
            item.MenuItemDescription.ShouldBe(menuItem.Description);
            item.Quantity.ShouldBe(orderItem.Quantity);
        }
    }
}
