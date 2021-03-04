using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.DeliverOrder;
using Web.Services.DateTimeServices;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;
using Order = WebTests.TestData.Order;
using OrderItem = WebTests.TestData.OrderItem;

namespace WebTests.Features.Orders.DeliverOrder
{
    public class DeliverOrderIntegrationTests : IntegrationTestBase
    {
        public DeliverOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Delivers_The_Order()
        {
            var restaurant = new Restaurant();

            restaurant.Menu.Categories.Add(new MenuCategory()
            {
                Items = new()
                {
                    new MenuItem(),
                },
            });

            var order = new Order()
            {
                Status = OrderStatus.Accepted,
                AcceptedAt = DateTime.UtcNow,
                Restaurant = restaurant,
                Items = new()
                {
                    new OrderItem()
                    {
                        MenuItem = restaurant.Menu
                            .Categories.First()
                            .Items.First(),
                    },
                },
            };

            Insert(restaurant, order);

            var now = DateTime.UtcNow;

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDateTimeProvider>(new DateTimeProviderStub() {UtcNow = now});
                });
            });

            var response = await factory.GetAuthenticatedClient(restaurant.Manager).Put(
                $"/orders/{order.Id}/deliver");

            response.StatusCode.ShouldBe(200);

            UseTestDbContext(db => db.Entry(order).Reload());

            order.DeliveredAt?.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var ev = UseTestDbContext(db => db.Events.Single().ToEvent()) as OrderDeliveredEvent;

            ev.OrderId.Value.ShouldBe(order.Id);
            ev.OccuredAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
