using System;
using Shouldly;
using Web;
using Web.Domain;
using Web.Domain.Baskets;
using Web.Domain.Billing;
using Web.Domain.Menus;
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
            var (restaurant, billingAccount, menu, basket, mobileNumber, deliveryLocation) = Setup();
            var menuItem = menu.Categories[0].Items[0];

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeSuccessful();

            var order = result.Value;

            order.Id.ShouldBe(orderId);
            order.UserId.ShouldBe(basket.UserId);
            order.RestaurantId.ShouldBe(restaurant.Id);
            order.Subtotal.ShouldBe(basket.CalculateSubtotal(menu));
            order.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            order.Status.ShouldBe(OrderStatus.Placed);
            order.MobileNumber.ShouldBe(mobileNumber);
            order.Address.ShouldBe(deliveryLocation.Address);
            order.PlacedAt.ShouldBe(now);
            order.Items.ShouldHaveSingleItem();
            order.Items[0].Name.ShouldBe(menuItem.Name);
            order.Items[0].Price.ShouldBe(menuItem.Price);
            order.Items[0].Quantity.ShouldBe(basket.Items[0].Quantity);
        }

        [Fact]
        public void It_Fails_If_The_Delivery_Location_Is_Out_Of_Range()
        {
            var (restaurant, billingAccount, menu, basket, mobileNumber, _) = Setup();

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -3));

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Billing_Account_Is_Disabled()
        {
            var (restaurant, billingAccount, menu, basket, mobileNumber, deliveryLocation) = Setup();

            billingAccount.Disable();

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Basket_Has_No_Items()
        {
            var (restaurant, billingAccount, menu, _, mobileNumber, deliveryLocation) = Setup();

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Restaurant_Is_Closed_At_Time_Of_Delivery()
        {
            var (restaurant, billingAccount, menu, basket, mobileNumber, deliveryLocation) = Setup();

            restaurant.OpeningTimes = null;

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public void It_Fails_If_The_Subtotal_Is_Less_Than_The_Min_Order()
        {
            var (restaurant, billingAccount, menu, basket, mobileNumber, deliveryLocation) = Setup();

            restaurant.MinimumDeliverySpend = Money.FromPounds(100.00m);

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var result = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        private static (
            Restaurant restaurant,
            BillingAccount billingAccount,
            Menu menu,
            Basket basket,
            MobileNumber mobileNumber,
            DeliveryLocation deliveryLocation) Setup()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
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
                MinimumDeliverySpend = Money.FromPounds(10.00m),
                MaxDeliveryDistance = Distance.FromKm(5)
            };

            var billingAccount = new BillingAccount(new BillingAccountId(Guid.NewGuid().ToString()));
            billingAccount.Enable();
            restaurant.AddBillingAccount(billingAccount.Id);

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var mobileNumber = new MobileNumber("07123456789");

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2));

            return (restaurant, billingAccount, menu, basket, mobileNumber, deliveryLocation);
        }
    }
}
