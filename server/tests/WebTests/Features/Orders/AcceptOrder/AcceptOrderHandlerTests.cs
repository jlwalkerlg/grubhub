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
        private readonly AcceptOrderHandler handler;
        private readonly UnitOfWorkSpy unitOfWork;
        private readonly AuthenticatorSpy authenticator;
        private readonly ClockStub clock;

        public AcceptOrderHandlerTests()
        {
            unitOfWork = new UnitOfWorkSpy();
            authenticator = new AuthenticatorSpy();
            clock = new ClockStub() {UtcNow = DateTime.UtcNow};

            handler = new AcceptOrderHandler(unitOfWork, authenticator, clock);
        }

        [Fact]
        public async Task It_Accepts_The_Order()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MaxDeliveryDistance = Distance.FromKm(10);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(),"Pizza");
            var (item, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                "Cheese & tomato",
                Money.FromPounds(9.99m));

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                "Bruno",
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
                    new Address("12 Maine Road, Manchester, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.UtcNow);

            order.Confirm(DateTime.UtcNow);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);

            await authenticator.SignIn(manager);

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            order.AlreadyAccepted.ShouldBeTrue();
            order.AcceptedAt.ShouldBe(clock.UtcNow);

            var ev = unitOfWork.EventRepositorySpy.Events.OfType<OrderAcceptedEvent>().Single();
            ev.OrderId.ShouldBe(order.Id);
            ev.CreatedAt.ShouldBe(clock.UtcNow);
        }

        [Fact]
        public async Task It_Is_Idempotent()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MaxDeliveryDistance = Distance.FromKm(10);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(),"Pizza");
            var (item, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                "Cheese & tomato",
                Money.FromPounds(9.99m));

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                "Bruno",
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
                    new Address("12 Maine Road, Manchester, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.UtcNow);

            order.Confirm(DateTime.UtcNow);

            var acceptedAt = DateTime.UtcNow;
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

            order.AlreadyAccepted.ShouldBeTrue();
            order.AcceptedAt.ShouldBe(acceptedAt);

            unitOfWork.EventRepositorySpy.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Returns_Unauthorised_If_The_Restaurant_Is_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MaxDeliveryDistance = Distance.FromKm(10);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(),"Pizza");
            var (item, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                "Cheese & tomato",
                Money.FromPounds(9.99m));

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                "Bruno",
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
                    new Address("12 Maine Road, Manchester, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.UtcNow);

            order.Confirm(DateTime.UtcNow);

            await unitOfWork.Orders.Add(order);

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
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MaxDeliveryDistance = Distance.FromKm(10);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(),"Pizza");
            var (item, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                "Cheese & tomato",
                Money.FromPounds(9.99m));

            var customer = new Customer(
                new UserId(Guid.NewGuid()),
                "Bruno",
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
                    new Address("12 Maine Road, Manchester, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.UtcNow);

            order.Confirm(DateTime.UtcNow);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Restaurants.Add(restaurant);

            await authenticator.SignIn(customer);

            var command = new AcceptOrderCommand()
            {
                OrderId = order.Id.Value,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError(ErrorType.Unauthorised);
        }
    }
}
