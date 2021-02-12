using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web;
using Web.Features.Billing;
using Web.Features.Orders.PlaceOrder;
using Web.Services;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.PlaceOrder
{
    public class PlaceOrderIntegrationTests : IntegrationTestBase
    {
        public PlaceOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Places_An_Order()
        {
            var now = DateTime.UtcNow;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };

            using var factory = fixture.CreateFactory(services =>
            {
                services.AddSingleton<IClock>(
                    new ClockStub()
                    {
                        UtcNow = now,
                    });

                services.AddSingleton<IBillingService>(
                    new BillingServiceSpy()
                    {
                        PaymentIntentResult = Result.Ok(paymentIntent),
                    });
            });

            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();
            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var basket = new Basket();
            basket.User = user;
            basket.Restaurant = restaurant;
            var basketItem = new BasketItem();
            basketItem.MenuItem = menuItem;
            basketItem.Quantity = 2;
            basket.Items.Add(basketItem);

            fixture.Insert(restaurant, user, basket);

            var client = new HttpTestClient(factory);

            var request = new PlaceOrderRequest()
            {
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                AddressLine3 = "Madchester",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var response = await client.Authenticate(user.Id).Post(
                $"/restaurants/{restaurant.Id}/orders",
                request);

            response.StatusCode.ShouldBe(200);

            var orderId = await response.GetData<string>();

            var order = fixture.UseTestDbContext(db => db.Orders.Single());

            order.Id.ShouldBe(orderId);
            order.UserId.ShouldBe(user.Id);
            order.RestaurantId.ShouldBe(restaurant.Id);
            order.Subtotal.ShouldBe(menuItem.Price * basketItem.Quantity);
            order.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            order.Status.ShouldBe(Web.Domain.Orders.OrderStatus.Placed);
            order.Address.ShouldBe("12 Maine Road, Oldham, Madchester, Manchester, MN12 1NM");
            order.PlacedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            order.PaymentIntentId.ShouldBe(paymentIntent.Id);
            order.PaymentIntentClientSecret.ShouldBe(paymentIntent.ClientSecret);

            var item = fixture.UseTestDbContext(db => db.OrderItems.Single());

            item.MenuItemId.ShouldBe(basketItem.MenuItemId);
            item.Quantity.ShouldBe(basketItem.Quantity);

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(OrderPlacedEvent).ToString());

            var opEvent = @event.ToEvent<OrderPlacedEvent>();

            opEvent.OrderId.Value.ShouldBe(order.Id);
            opEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
