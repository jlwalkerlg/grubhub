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
    public class ConfirmOrderHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly ClockStub clockStub;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly ConfirmOrderHandler handler;

        public ConfirmOrderHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            clockStub = new ClockStub();

            billingServiceSpy = new BillingServiceSpy();

            handler = new ConfirmOrderHandler(
                unitOfWorkSpy,
                clockStub,
                billingServiceSpy);
        }

        [Fact]
        public async Task It_Confirms_The_Order()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road"),
                new Coordinates(54, -2));

            restaurant.MaxDeliveryDistanceInKm = 10;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, new Money(15m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("13 Maine Road"),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var now = DateTime.UtcNow;

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                deliveryLocation,
                billingAccount,
                now).Value;

            order.PaymentIntentId = Guid.NewGuid().ToString();

            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.Orders.Add(order);

            clockStub.UtcNow = now;

            billingServiceSpy.ConfirmResult = Result.Ok();

            var command = new ConfirmOrderCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.PaymentConfirmed);

            unitOfWorkSpy.BasketRepositorySpy.Baskets.ShouldBeEmpty();

            var ocEvent = unitOfWorkSpy.EventRepositorySpy
                .Events
                .OfType<OrderConfirmedEvent>()
                .Single();

            ocEvent.OrderId.ShouldBe(order.Id);
            ocEvent.CreatedAt.ShouldBe(now);

            billingServiceSpy.ConfirmedOrder.ShouldBe(order);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road"),
                new Coordinates(54, -2));

            restaurant.MaxDeliveryDistanceInKm = 10;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, new Money(15m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("13 Maine Road"),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var now = DateTime.UtcNow;

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                deliveryLocation,
                billingAccount,
                now).Value;

            order.PaymentIntentId = Guid.NewGuid().ToString();

            order.Confirm();

            await unitOfWorkSpy.Orders.Add(order);

            var command = new ConfirmOrderCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.PaymentConfirmed);

            unitOfWorkSpy.EventRepositorySpy.Events.ShouldBeEmpty();

            billingServiceSpy.ConfirmedOrder.ShouldBeNull();
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var command = new ConfirmOrderCommand()
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
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road"),
                new Coordinates(54, -2));

            restaurant.MaxDeliveryDistanceInKm = 10;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, new Money(15m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("13 Maine Road"),
                new Coordinates(54, -2));

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var now = DateTime.UtcNow;

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                deliveryLocation,
                billingAccount,
                now).Value;

            order.PaymentIntentId = Guid.NewGuid().ToString();

            await unitOfWorkSpy.Orders.Add(order);

            clockStub.UtcNow = now;

            billingServiceSpy.ConfirmResult = Error.BadRequest("Order not confirmed.");

            var command = new ConfirmOrderCommand()
            {
                PaymentIntentId = order.PaymentIntentId,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.ShouldBe(billingServiceSpy.ConfirmResult.Error);
        }
    }
}