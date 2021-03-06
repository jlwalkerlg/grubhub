﻿using System;
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

            // second
            var order1 = new Order()
            {
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow,
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 3,
                    }
                }
            };

            // first
            var order2 = new Order()
            {
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow.AddSeconds(-5),
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 4,
                    }
                }
            };

            // skipped
            var order3 = new Order()
            {
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTime.UtcNow.AddSeconds(-10),
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 1,
                    }
                }
            };

            // not confirmed
            var order4 = new Order()
            {
                Status = OrderStatus.Placed,
                PlacedAt = DateTime.UtcNow,
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 2,
                    }
                }
            };

            // delivered
            var order5 = new Order()
            {
                Status = OrderStatus.Delivered,
                ConfirmedAt = DateTime.UtcNow,
                DeliveredAt = DateTime.UtcNow,
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = menuItem,
                        Quantity = 2,
                    }
                }
            };

            Insert(restaurant, order1, order2, order3, order4, order5);

            var confirmedAfter = order3.ConfirmedAt.Value.ToString("O");

            var response = await factory.GetAuthenticatedClient(restaurant.Manager).Get(
                $"/restaurant/active-orders?confirmedAfter={confirmedAfter}");

            if (!response.IsSuccessStatusCode)
            {
                output.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<GetActiveRestaurantOrdersAction.OrderModel[]>();

            data.Length.ShouldBe(2);

            data[0].Id.ShouldBe(order2.Id);
            data[1].Id.ShouldBe(order1.Id);
        }
    }
}