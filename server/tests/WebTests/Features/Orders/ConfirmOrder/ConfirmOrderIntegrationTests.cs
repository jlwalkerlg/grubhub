using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web;
using Web.Features.Billing;
using Web.Features.Orders.ConfirmOrder;
using Web.Services;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderIntegrationTests : IntegrationTestBase
    {
        public ConfirmOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Confirms_An_Order()
        {
            var now = DateTime.UtcNow;

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
                        ConfirmResult = Result.Ok(),
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
            order.Status = Web.Domain.Orders.OrderStatus.Placed;

            fixture.Insert(restaurant, user, order);

            var client = new HttpTestClient(factory);

            var response = await client.Authenticate(user.Id).Post(
                $"/orders/{order.Id}/confirm");

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.Orders.Single());

            found.Status.ShouldBe(Web.Domain.Orders.OrderStatus.PaymentConfirmed);
            found.ConfirmedAt.Value.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(OrderConfirmedEvent).ToString());

            var ocEvent = @event.ToEvent<OrderConfirmedEvent>();

            ocEvent.OrderId.Value.ShouldBe(order.Id);
            ocEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
