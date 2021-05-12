using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Baskets;
using Web.Domain.Billing;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Orders.ConfirmOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderByPaymentIntentIdHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly DateTimeProviderStub dateTimeProviderStub;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly ConfirmOrderByPaymentIntentIdHandler handler;

        private readonly DateTimeOffset now = DateTimeOffset.Now;

        public ConfirmOrderByPaymentIntentIdHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            dateTimeProviderStub = new DateTimeProviderStub();
            billingServiceSpy = new BillingServiceSpy();

            handler = new ConfirmOrderByPaymentIntentIdHandler(
                unitOfWorkSpy,
                dateTimeProviderStub,
                billingServiceSpy);
        }

        [Fact]
        public async Task It_Confirms_The_Order()
        {
            var order = await SetupOrder();

            var command = new ConfirmOrderByPaymentIntentIdCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.PaymentConfirmed);
            order.ConfirmedAt.ShouldBe(now);

            unitOfWorkSpy.BasketRepositorySpy.Baskets.ShouldBeEmpty();

            var ocEvent = unitOfWorkSpy
                .Events
                .OfType<OrderConfirmedEvent>()
                .Single();

            ocEvent.OrderId.ShouldBe(order.Id);
            ocEvent.OccuredAt.ShouldBe(now);

            billingServiceSpy.ConfirmedOrder.ShouldBe(order);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var order = await SetupOrder();
            order.Confirm(DateTimeOffset.UtcNow);

            var command = new ConfirmOrderByPaymentIntentIdCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.PaymentConfirmed);

            unitOfWorkSpy.Events.ShouldBeEmpty();

            billingServiceSpy.ConfirmedOrder.ShouldBeNull();
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var command = new ConfirmOrderByPaymentIntentIdCommand()
            {
                PaymentIntentId = Guid.NewGuid().ToString(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Billing_Service_Fails_To_Confirm_The_Order()
        {
            var order = await SetupOrder();

            billingServiceSpy.PaymentAccepted = false;

            var command = new ConfirmOrderByPaymentIntentIdCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
        }

        private async Task<Order> SetupOrder()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2))
            {
                MaxDeliveryDistance = Distance.FromKm(10),
                MinimumDeliverySpend = Money.FromPounds(10m),
                OpeningTimes = OpeningTimes.Always
            };

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(15m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(new BillingAccountId(Guid.NewGuid().ToString()));
            billingAccount.Enable();

            restaurant.AddBillingAccount(billingAccount.Id);

            var order = restaurant.PlaceOrder(
                    new OrderId(Guid.NewGuid().ToString()),
                    basket,
                    menu,
                    new MobileNumber("07123456789"),
                    deliveryLocation,
                    billingAccount,
                    now,
                    TimeZoneInfo.Utc)
                .Value;

            order.PaymentIntentId = Guid.NewGuid().ToString();

            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.Orders.Add(order);

            dateTimeProviderStub.UtcNow = now;

            billingServiceSpy.PaymentAccepted = true;

            return order;
        }
    }
}
