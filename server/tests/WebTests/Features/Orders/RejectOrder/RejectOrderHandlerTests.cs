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
using Web.Features.Orders.RejectOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.RejectOrder
{
    public class RejectOrderHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly AuthenticatorSpy authenticator;
        private readonly DateTimeProviderStub dateTimeProvider;
        private readonly RejectOrderHandler handler;

        public RejectOrderHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            authenticator = new AuthenticatorSpy();
            dateTimeProvider = new DateTimeProviderStub() { UtcNow = DateTimeOffset.UtcNow };

            handler = new RejectOrderHandler(unitOfWork, authenticator, dateTimeProvider);
        }

        [Fact]
        public async Task It_Rejects_The_Order()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Rejected);
            order.Rejected.ShouldBeTrue();
            order.RejectedAt.ShouldBe(dateTimeProvider.UtcNow);

            var evnt = unitOfWork.Events
                .OfType<OrderRejectedEvent>()
                .Single();

            evnt.OrderId.ShouldBe(order.Id.Value);
            evnt.OccuredAt.ShouldBe(dateTimeProvider.UtcNow);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var (manager, restaurant, order) = SetupOrder();

            order.Reject(DateTimeOffset.UtcNow);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Rejected);
            order.Rejected.ShouldBeTrue();

            unitOfWork.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Was_Not_Yet_Confirmed()
        {
            var (manager, restaurant, order) = SetupOrder(confirm: false);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Was_Already_Accepted()
        {
            var (manager, restaurant, order) = SetupOrder();

            order.Accept(DateTimeOffset.UtcNow);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Was_Not_Found()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Was_Not_Found()
        {
            var (manager, _, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Returns_Unauthorised_If_The_Authenticated_User_Is_Not_The_Manager()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(Guid.NewGuid());

            var command = new RejectOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.Unauthorised);
        }

        private static (RestaurantManager manager, Restaurant restaurant, Order order) SetupOrder(bool confirm = true)
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
                DateTimeOffset.UtcNow,
                TimeZoneInfo.Utc);

            if (confirm) order.Confirm(DateTimeOffset.UtcNow);

            return (manager, restaurant, order);
        }
    }
}
