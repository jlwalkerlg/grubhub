using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.GetActiveRestaurantOrders;
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
        private readonly ITestOutputHelper output;

        public GetActiveRestaurantOrdersIntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            this.output = output;
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

            output.WriteLine(response.Content.ReadAsStringAsync().Result);

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<GetActiveRestaurantOrdersAction.OrderModel[]>();

            data.Length.ShouldBe(2);

            data[0].Id.ShouldBe(order4.Id);
            data[1].Id.ShouldBe(order3.Id);
        }
    }
}
