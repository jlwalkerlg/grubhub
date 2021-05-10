using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Domain.Orders;
using Web.Features.Orders.RejectOrder;
using Web.Services.DateTimeServices;
using WebTests.Doubles;
using Xunit;
using Order = WebTests.TestData.Order;

namespace WebTests.Features.Orders.RejectOrder
{
    public class RejectOrderIntegrationTests : IntegrationTestBase

    {
        public RejectOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Rejects_The_Order()
        {
            var order = new Order()
            {
                Status = OrderStatus.PaymentConfirmed,
                ConfirmedAt = DateTimeOffset.UtcNow.AddMinutes(-1),
            };

            Insert(order);

            var now = DateTimeOffset.UtcNow;

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDateTimeProvider>(
                        new DateTimeProviderStub() { UtcNow = now });
                });
            });

            var response = await factory.GetAuthenticatedClient(order.Restaurant.ManagerId).Put(
                $"/orders/{order.Id}/reject");

            response.StatusCode.ShouldBe(200);

            Reload(order);

            order.Status.ShouldBe(OrderStatus.Rejected);
            order.RejectedAt?.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var outbox = factory.Services.GetRequiredService<OutboxSpy>();
            var ev = outbox.Events.OfType<OrderRejectedEvent>().Single();

            ev.OrderId.ShouldBe(order.Id);
            ev.OccuredAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
