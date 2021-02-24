using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.GetActiveOrder;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.GetRestaurantOrders
{
    public class GetRestaurantOrdersIntegrationTests : IntegrationTestBase
    {
        public GetRestaurantOrdersIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Gets_The_Restaurants_Orders()
        {
            var restaurant = new Restaurant();

            var menu = restaurant.Menu;
            var menuCategory = new MenuCategory();
            var menuItem = new MenuItem();
            menuCategory.Items.Add(menuItem);
            menu.Categories.Add(menuCategory);

            var order1 = new Order()
            {
                Restaurant = restaurant,
            };
            var orderItem1 = new OrderItem()
            {
                MenuItem = menuItem,
            };
            order1.Items.Add(orderItem1);

            var order2 = new Order()
            {
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                    }
                }
            };

            Insert(restaurant, order1, order2);

            var response = await factory.GetAuthenticatedClient(restaurant.Manager).Get(
                "/restaurant/orders");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto[]>();

            data.Length.ShouldBe(2);

            data[0].Id.ShouldBe(order1.Id);
            data[1].Id.ShouldBe(order2.Id);

            var orderDto = data[0];

            orderDto.UserId.ShouldBe(order1.UserId);
            orderDto.RestaurantId.ShouldBe(order1.RestaurantId);
            orderDto.Subtotal.ShouldBe(order1.Subtotal);
            orderDto.DeliveryFee.ShouldBe(order1.DeliveryFee);
            orderDto.ServiceFee.ShouldBe(order1.ServiceFee);
            orderDto.Status.ShouldBe(order1.Status);
            orderDto.Address.ShouldBe(order1.Address);
            orderDto.PlacedAt.ShouldBe(order1.PlacedAt, TimeSpan.FromSeconds(0.000001));
            orderDto.RestaurantName.ShouldBe(restaurant.Name);
            orderDto.RestaurantAddress.ShouldBe(restaurant.Address);
            orderDto.RestaurantPhoneNumber.ShouldBe(restaurant.PhoneNumber);
            orderDto.Items.ShouldHaveSingleItem();

            var itemDto = orderDto.Items.Single();

            itemDto.MenuItemId.ShouldBe(menuItem.Id);
            itemDto.MenuItemName.ShouldBe(menuItem.Name);
            itemDto.MenuItemPrice.ShouldBe(menuItem.Price);
            itemDto.MenuItemDescription.ShouldBe(menuItem.Description);
            itemDto.Quantity.ShouldBe(orderItem1.Quantity);
        }
    }
}
