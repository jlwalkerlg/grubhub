using System;
using Shouldly;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Domain.Orders
{
    public class OrderTests
    {
        [Fact]
        public void Changing_A_Placed_Order_Sets_Its_Status_Back_To_Active()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road"),
                new Coordinates(54, -2)
            );
            restaurant.MinimumDeliverySpend = new Money(10m);
            restaurant.MaxDeliveryDistanceInKm = 1;
            restaurant.OpeningTimes = OpeningTimes.Always;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            restaurant.PlaceOrder(
                new Money(15m),
                order,
                new DeliveryLocation(
                    new Address("13 Maine Road"),
                    new Coordinates(54, -2)
                ),
                billingAccount,
                DateTime.UtcNow
            );

            order.Status.ShouldBe(OrderStatus.Placed);

            var menuItemId = Guid.NewGuid();
            order.AddItem(menuItemId, 1);

            order.Status.ShouldBe(OrderStatus.Active);

            restaurant.PlaceOrder(
                new Money(15m),
                order,
                new DeliveryLocation(
                    new Address("13 Maine Road"),
                    new Coordinates(54, -2)
                ),
                billingAccount,
                DateTime.UtcNow
            );

            order.Status.ShouldBe(OrderStatus.Placed);

            order.RemoveItem(menuItemId);

            order.Status.ShouldBe(OrderStatus.Active);
        }
    }
}
