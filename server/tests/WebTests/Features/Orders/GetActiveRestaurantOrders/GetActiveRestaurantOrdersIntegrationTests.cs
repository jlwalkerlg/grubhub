using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.GetActiveOrder;
using WebTests.TestData;
using Xunit;
using Xunit.Abstractions;
using Order = WebTests.TestData.Order;
using OrderItem = WebTests.TestData.OrderItem;
using Restaurant = WebTests.TestData.Restaurant;

namespace WebTests.Features.Orders.GetActiveRestaurantOrders
{
    public class GetActiveRestaurantOrdersIntegrationTests : IntegrationTestBase
    {
        public GetActiveRestaurantOrdersIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Sorts_And_Filters_The_Restaurants_Active_Orders()
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
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow.AddSeconds(-10),
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 1,
                    }
                }
            };

            var order2 = new Order()
            {
                Restaurant = restaurant,
                Status = OrderStatus.Placed,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 2,
                    }
                }
            };

            var order3 = new Order()
            {
                Restaurant = restaurant,
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 3,
                    }
                }
            };

            var order4 = new Order()
            {
                Restaurant = restaurant,
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow.AddSeconds(-5),
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 4,
                    }
                }
            };

            Insert(restaurant, order1, order2, order3, order4);

            var confirmedAfter = order1.ConfirmedAt.Value.ToString("O");

            var response = await factory.GetAuthenticatedClient(restaurant.Manager).Get(
                $"/restaurant/active-orders?confirmedAfter={confirmedAfter}");

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<OrderDto[]>();

            data.Length.ShouldBe(2);

            data[0].Id.ShouldBe(order4.Id);
            data[1].Id.ShouldBe(order3.Id);

            data[0].Number.ShouldBe(order4.Number);
            data[0].UserId.ShouldBe(order4.UserId);
            data[0].RestaurantId.ShouldBe(order4.RestaurantId);
            data[0].Subtotal.ShouldBe(order4.Subtotal);
            data[0].DeliveryFee.ShouldBe(order4.DeliveryFee);
            data[0].ServiceFee.ShouldBe(order4.ServiceFee);
            data[0].Status.ShouldBe(order4.Status);
            data[0].Address.ShouldBe(order4.Address);
            data[0].PlacedAt.ShouldBe(order4.PlacedAt, TimeSpan.FromSeconds(0.000001));
            data[0].ConfirmedAt.ShouldBe(order4.ConfirmedAt);
            data[0].RestaurantName.ShouldBe(order4.Restaurant.Name);
            data[0].RestaurantAddress.ShouldBe(order4.Restaurant.Address);
            data[0].RestaurantPhoneNumber.ShouldBe(order4.Restaurant.PhoneNumber);
            data[0].Items.ShouldHaveSingleItem();

            data[0].Items[0].MenuItemId.ShouldBe(order4.Items[0].MenuItem.Id);
            data[0].Items[0].MenuItemName.ShouldBe(order4.Items[0].MenuItem.Name);
            data[0].Items[0].MenuItemPrice.ShouldBe(order4.Items[0].MenuItem.Price);
            data[0].Items[0].MenuItemDescription.ShouldBe(order4.Items[0].MenuItem.Description);
            data[0].Items[0].Quantity.ShouldBe(order4.Items[0].Quantity);
        }
    }
}
