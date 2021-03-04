using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Features.Orders.AcceptOrder;
using Web.Services.DateTimeServices;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.AcceptOrder
{
    public class AcceptOrderIntegrationTests : IntegrationTestBase
    {
        public AcceptOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Accepts_The_Order()
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
                $"/orders/{order.Id}/accept");

            response.StatusCode.ShouldBe(200);

            UseTestDbContext(db => db.Entry(order).Reload());

            order.AcceptedAt?.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var ev = UseTestDbContext(db => db.Events.Single().ToEvent()) as OrderAcceptedEvent;

            ev.OrderId.Value.ShouldBe(order.Id);
            ev.OccuredAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
