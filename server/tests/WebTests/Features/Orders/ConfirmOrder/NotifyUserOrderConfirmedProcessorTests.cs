using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
    public class NotifyUserOrderConfirmedProcessorTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly NotifierSpy notifierSpy;
        private readonly NotifyUserOrderConfirmedProcessor processor;

        public NotifyUserOrderConfirmedProcessorTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            notifierSpy = new NotifierSpy();

            processor = new NotifyUserOrderConfirmedProcessor(
                unitOfWorkSpy,
                notifierSpy);
        }

        [Fact]
        public async Task It_Notifies_The_User()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
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
            await unitOfWorkSpy.Users.Add(user);

            var job = new NotifyUserOrderConfirmedJob(order.Id);

            await processor.Handle(job, default);

            notifierSpy.Customers[user].Single().ShouldBe(order);
        }
    }
}
