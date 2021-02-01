using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Orders.SubmitOrder;
using Web.Services;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.SubmitOrder
{
    public class SubmitOrderIntegrationTests : IntegrationTestBase
    {
        public SubmitOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Submits_An_Order()
        {
            var now = DateTime.UtcNow;

            using var factory = fixture.CreateFactory(services =>
            {
                services.AddSingleton<IClock>(new ClockStub()
                {
                    UtcNow = now,
                });
            });

            var client = new HttpTestClient(factory);

            var restaurant = new Restaurant();

            var menuItem = new MenuItem();
            var menuCategory = new MenuCategory() { Items = new() { menuItem } };
            var menu = new Menu() { Restaurant = restaurant, Categories = new() { menuCategory } };

            var user = new User();

            var orderItem = new OrderItem()
            {
                MenuItem = menuItem,
                Quantity = 3,
            };

            var order = new Order()
            {
                User = user,
                Restaurant = restaurant,
                Items = new() { orderItem },
            };

            fixture.Insert(restaurant, menu, user, order);

            var response = await client.Authenticate(user.Id).Put(
                $"/order/{restaurant.Id}/submit");

            response.StatusCode.ShouldBe(200);

            var found = fixture.UseTestDbContext(db => db.Orders.Single());

            found.Status.ShouldBe(Web.Domain.Orders.OrderStatus.Submitted);

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(OrderSubmittedEvent).ToString());
            @event.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var oEvent = @event.ToEvent<OrderSubmittedEvent>();

            oEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            oEvent.OrderId.Value.ShouldBe(order.Id);
        }
    }
}
