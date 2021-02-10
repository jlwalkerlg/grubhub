using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Billing;
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
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly ClockStub clockStub;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly ConfirmOrderHandler handler;

        public ConfirmOrderHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            authenticatorSpy = new AuthenticatorSpy();

            clockStub = new ClockStub();

            billingServiceSpy = new BillingServiceSpy();

            handler = new ConfirmOrderHandler(
                unitOfWorkSpy,
                authenticatorSpy,
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
                new Coordinates(54, -2)
            );
            restaurant.MaxDeliveryDistanceInKm = 10;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.OpeningTimes = OpeningTimes.Always;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );
            order.AddItem(Guid.NewGuid(), 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("13 Maine Road"),
                new Coordinates(54, -2.01f)
            );

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var now = DateTime.UtcNow;

            restaurant.PlaceOrder(
                new Money(15m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            await unitOfWorkSpy.Orders.Add(order);

            clockStub.UtcNow = now;

            authenticatorSpy.SignIn(order.UserId);

            billingServiceSpy.ConfirmResult = Result.Ok();

            var command = new ConfirmOrderCommand()
            {
                OrderId = order.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.PaymentConfirmed);
            order.ConfirmedAt.ShouldBe(now);

            var ocEvent = unitOfWorkSpy.EventRepositorySpy
                .Events
                .OfType<OrderConfirmedEvent>()
                .Single();

            ocEvent.OrderId.ShouldBe(order.Id);
            ocEvent.CreatedAt.ShouldBe(now);

            billingServiceSpy.ConfirmedOrder.ShouldBe(order);
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
                new Coordinates(54, -2)
            );
            restaurant.MaxDeliveryDistanceInKm = 10;
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.OpeningTimes = OpeningTimes.Always;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );
            order.AddItem(Guid.NewGuid(), 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("13 Maine Road"),
                new Coordinates(54, -2.01f)
            );

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var now = DateTime.UtcNow;

            restaurant.PlaceOrder(
                new Money(15m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            await unitOfWorkSpy.Orders.Add(order);

            clockStub.UtcNow = now;

            authenticatorSpy.SignIn(order.UserId);

            billingServiceSpy.ConfirmResult = Error.BadRequest("Already confirmed.");

            var command = new ConfirmOrderCommand()
            {
                OrderId = order.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Has_Not_Been_Placed()
        {
            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new RestaurantId(Guid.NewGuid())
            );

            await unitOfWorkSpy.Orders.Add(order);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            authenticatorSpy.SignIn(order.UserId);

            var command = new ConfirmOrderCommand()
            {
                OrderId = order.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var command = new ConfirmOrderCommand()
            {
                OrderId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new RestaurantId(Guid.NewGuid())
            );

            await unitOfWorkSpy.Orders.Add(order);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new ConfirmOrderCommand()
            {
                OrderId = order.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }
    }
}
