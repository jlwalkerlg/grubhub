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

            var fixture = this.fixture.WithServices(services =>
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

            var order = new Order();

            fixture.Insert(order);

            var command = new ConfirmOrderCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await fixture.Send(command);

            if (!result) throw new Exception(result.Error.Message);

            result.ShouldBeSuccessful();

            var found = fixture.UseTestDbContext(db => db.Orders.Single());

            found.Status.ShouldBe(Web.Domain.Orders.OrderStatus.PaymentConfirmed);

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(OrderConfirmedEvent).ToString());

            var ocEvent = @event.ToEvent<OrderConfirmedEvent>();

            ocEvent.OrderId.Value.ShouldBe(order.Id);
            ocEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
