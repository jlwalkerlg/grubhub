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

            var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
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
            });

            var basket = new Basket();

            var order = new Order();
            order.User = basket.User;
            order.Restaurant = basket.Restaurant;

            Insert(basket, order);

            var command = new ConfirmOrderCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await factory.Send(command);

            result.ShouldBeSuccessful();

            var baskets = UseTestDbContext(db => db.Baskets.ToArray());

            baskets.ShouldBeEmpty();

            var found = UseTestDbContext(db => db.Orders.Single());

            found.Status.ShouldBe(Web.Domain.Orders.OrderStatus.PaymentConfirmed);

            var ev = UseTestDbContext(db => db.Events.Single());

            var ocEv = ev.ToEvent() as OrderConfirmedEvent;

            ocEv.OrderId.Value.ShouldBe(order.Id);
            ocEv.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
        }
    }
}
