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
using Web.Features.Orders.CancelOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.CancelOrder
{
    public class CancelOrderHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly AuthenticatorSpy authenticator;
        private readonly DateTimeProviderStub dateTimeProvider;
        private readonly EventBusSpy bus = new();
        private readonly CancelOrderHandler handler;

        public CancelOrderHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            authenticator = new AuthenticatorSpy();
            dateTimeProvider = new DateTimeProviderStub() {UtcNow = DateTime.UtcNow};

            handler = new CancelOrderHandler(unitOfWork, authenticator, dateTimeProvider, bus);
        }

        [Fact]
        public async Task It_Cancels_The_Order()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Cancelled);
            order.Cancelled.ShouldBeTrue();
            order.CancelledAt.ShouldBe(dateTimeProvider.UtcNow);

            var evnt = bus.Events
                .OfType<OrderCancelledEvent>()
                .Single();

            evnt.OrderId.ShouldBe(order.Id.Value);
            evnt.OccuredAt.ShouldBe(dateTimeProvider.UtcNow);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var (manager, restaurant, order) = SetupOrder();

            order.Cancel(DateTime.Now);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Cancelled);
            order.Cancelled.ShouldBeTrue();

            bus.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Was_Not_Yet_Accepted()
        {
            var (manager, restaurant, order) = SetupOrder(accept: false);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Was_Already_Delivered()
        {
            var (manager, restaurant, order) = SetupOrder();

            order.Deliver(DateTime.Now);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Authenticated_User_Is_Not_The_Restaurant_Manager()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(Guid.NewGuid());

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new CancelOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.NotFound);
        }

        private static (RestaurantManager manager, Restaurant restaurant, Order order) SetupOrder(bool accept = true)
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan",
                "Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2))
            {
                OpeningTimes = OpeningTimes.Always,
                MaxDeliveryDistance = Distance.FromKm(10),
            };

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(), "Pizza");
            var (item, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                "Cheese & tomato",
                Money.FromPounds(9.99m));

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                "Bruno",
                "Walker",
                new Email("bruno@gmail.com"),
                "password123");

            var basket = new Basket(customer.Id, restaurant.Id);
            basket.AddItem(item.Id, 5);

            var (order, _) = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                new DeliveryLocation(
                    new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.UtcNow);

            order.Confirm(DateTime.Now);

            if (accept) order.Accept(DateTime.Now);

            return (manager, restaurant, order);
        }
    }
}
