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
using Web.Features.Orders.AcceptOrder;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Orders.AcceptOrder
{
    public class AcceptOrderHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly AuthenticatorSpy authenticator;
        private readonly DateTimeProviderStub dateTimeProvider;
        private readonly AcceptOrderHandler handler;

        public AcceptOrderHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            authenticator = new AuthenticatorSpy();
            dateTimeProvider = new DateTimeProviderStub() { UtcNow = DateTimeOffset.UtcNow };

            handler = new AcceptOrderHandler(unitOfWork, authenticator, dateTimeProvider);
        }

        [Fact]
        public async Task It_Accepts_The_Order()
        {
            var (manager, restaurant, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);

            await authenticator.SignIn(manager);

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Accepted.ShouldBeTrue();
            order.AcceptedAt.ShouldBe(dateTimeProvider.UtcNow);

            var ev = unitOfWork.OutboxSpy.Events.OfType<OrderAcceptedEvent>().Single();
            ev.OrderId.ShouldBe(order.Id);
            ev.OccuredAt.ShouldBe(dateTimeProvider.UtcNow);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var (manager, restaurant, order) = SetupOrder();

            var acceptedAt = DateTimeOffset.UtcNow;
            order.Accept(acceptedAt);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);

            await authenticator.SignIn(manager);

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.Accepted.ShouldBeTrue();
            order.AcceptedAt.ShouldBe(acceptedAt);

            unitOfWork.OutboxSpy.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var (manager, _, order) = SetupOrder();

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new AcceptOrderCommand()
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

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Yet_Confirmed()
        {
            var (manager, restaurant, order) = SetupOrder(confirm: false);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);
            await unitOfWork.Users.Add(manager);

            await authenticator.SignIn(manager);

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.BadRequest);
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
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MaxDeliveryDistance = Distance.FromKm(10);

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
                DateTimeOffset.UtcNow);

            if (confirm) order.Confirm(DateTimeOffset.UtcNow);

            return (manager, restaurant, order);
        }
    }
}
