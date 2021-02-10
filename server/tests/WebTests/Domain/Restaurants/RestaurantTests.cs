using System;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Domain.Restaurants
{
    public class RestaurantTests
    {
        [Fact]
        public void It_Places_The_Order()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.Now;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Placed);
            order.PlacedAt.ShouldBe(now);
            order.Address.ShouldBe(deliveryAddress);

            deliveryAddress = new Address("13 Maine Road, Manchester, UK, MN12 1NM");

            deliveryCoordinates = new Coordinates(54, -2.01f);

            deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            now = DateTime.Now;

            result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeSuccessful();

            order.Status.ShouldBe(OrderStatus.Placed);
            order.PlacedAt.ShouldBe(now);
            order.Address.ShouldBe(deliveryAddress);
        }

        [Fact]
        public void It_Fails_If_The_Delivery_Location_Is_Out_Of_Range()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(53, -3);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.Now;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Billing_Account_Is_Disabled()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Disable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.Now;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Order_Has_No_Items()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.UtcNow;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Restaurant_Is_Closed_At_Time_Of_Delivery()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = null;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.UtcNow;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Subtotal_Is_Less_Than_The_Min_Order()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(Guid.NewGuid(), 1);

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.UtcNow;

            var result = restaurant.PlaceOrder(
                new Money(7.50m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Order_Is_Cancelled()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.MaxDeliveryDistanceInKm = 5;

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.Cancel();

            var deliveryAddress = new Address("12 Maine Road, Manchester, UK, MN12 1NM");

            var deliveryCoordinates = new Coordinates(54, -2);

            var deliveryLocation = new DeliveryLocation(
                deliveryAddress,
                deliveryCoordinates
            );

            var now = DateTime.UtcNow;

            var result = restaurant.PlaceOrder(
                new Money(15.00m),
                order,
                deliveryLocation,
                billingAccount,
                now
            );

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
