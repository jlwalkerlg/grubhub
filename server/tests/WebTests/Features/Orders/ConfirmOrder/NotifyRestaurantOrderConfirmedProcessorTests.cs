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
    public class NotifyRestaurantOrderConfirmedProcessorTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly NotifierSpy notifierSpy;
        private readonly NotifyRestaurantOrderConfirmedProcessor processor;

        public NotifyRestaurantOrderConfirmedProcessorTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            notifierSpy = new NotifierSpy();

            processor = new NotifyRestaurantOrderConfirmedProcessor(
                unitOfWorkSpy,
                notifierSpy);
        }

        [Fact]
        public async Task It_Notifies_The_Restaurant()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Mr Manager",
                new Email("mr@manager.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = Money.FromPounds(10.00m);
            restaurant.MaxDeliveryDistance = Distance.FromKm(5);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var user = new Customer(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker@jordan.com"),
                "password123");

            var basket = new Basket(
                user.Id,
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                new DeliveryLocation(
                    new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.Now).Value;

            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Users.Add(manager);

            var job = new NotifyRestaurantOrderConfirmedJob(order.Id);

            var result = await processor.Handle(job, default);

            result.ShouldBeSuccessful();

            notifierSpy.Managers[manager].Single().ShouldBe(order);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Mr Manager",
                new Email("mr@manager.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = Money.FromPounds(10.00m);
            restaurant.MaxDeliveryDistance = Distance.FromKm(5);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var user = new Customer(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker@jordan.com"),
                "password123");

            var basket = new Basket(
                user.Id,
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                new DeliveryLocation(
                    new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.Now).Value;

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Users.Add(manager);

            var job = new NotifyRestaurantOrderConfirmedJob(order.Id);

            var result = await processor.Handle(job, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);

            notifierSpy.Managers.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Mr Manager",
                new Email("mr@manager.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = Money.FromPounds(10.00m);
            restaurant.MaxDeliveryDistance = Distance.FromKm(5);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var user = new Customer(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker@jordan.com"),
                "password123");

            var basket = new Basket(
                user.Id,
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                new DeliveryLocation(
                    new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.Now).Value;

            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.Users.Add(manager);

            var job = new NotifyRestaurantOrderConfirmedJob(order.Id);

            var result = await processor.Handle(job, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);

            notifierSpy.Managers.ShouldBeEmpty();
        }

        [Fact]
        public async Task It_Fails_If_The_Manager_Is_Not_Found()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Mr Manager",
                new Email("mr@manager.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = Money.FromPounds(10.00m);
            restaurant.MaxDeliveryDistance = Distance.FromKm(5);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var user = new Customer(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker@jordan.com"),
                "password123");

            var basket = new Basket(
                user.Id,
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var order = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber("07123456789"),
                new DeliveryLocation(
                    new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                    new Coordinates(54, -2)),
                billingAccount,
                DateTime.Now).Value;

            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var job = new NotifyRestaurantOrderConfirmedJob(order.Id);

            var result = await processor.Handle(job, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);

            notifierSpy.Managers.ShouldBeEmpty();
        }
    }
}
