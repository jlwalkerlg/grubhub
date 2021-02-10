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

            var order = new Order();
            order.User = user;
            order.Restaurant = restaurant;

            var orderItem = new OrderItem();
            orderItem.MenuItem = menuItem;
            orderItem.Quantity = 2;

            order.Items.Add(orderItem);

            fixture.Insert(restaurant, user, order);

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
                $"/orders/{order.Id}/place",
                request);

            response.StatusCode.ShouldBe(200);

            var data = await response.GetData<string>();

            data.ShouldBe(paymentIntent.ClientSecret);

            var found = fixture.UseTestDbContext(db => db.Orders.Single());

            found.Status.ShouldBe(Web.Domain.Orders.OrderStatus.Placed);
            found.Address.ShouldBe("12 Maine Road, Oldham, Madchester, Manchester, MN12 1NM");
            found.PlacedAt.Value.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            found.PaymentIntentId.ShouldBe(paymentIntent.Id);

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(OrderPlacedEvent).ToString());

            var opEvent = @event.ToEvent<OrderPlacedEvent>();

            opEvent.OrderId.Value.ShouldBe(order.Id);
            opEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
