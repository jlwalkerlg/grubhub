using System;
using Shouldly;
using Web.Domain;
using Web.Domain.Baskets;
using Web.Domain.Billing;
using Web.Domain.Menus;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Domain.Orders
{
    public class OrderTests
    {
        [Fact]
        public void Subtotal_Is_Correct()
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
                new Coordinates(54, -2));

            restaurant.OpeningTimes = OpeningTimes.Always;
            restaurant.MinimumDeliverySpend = Money.FromPounds(10.00m);
            restaurant.MaxDeliveryDistance = Distance.FromKm(5);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id);

            billingAccount.Enable();

            var menu = new Menu(restaurant.Id);
            var (category, _) = menu.AddCategory(Guid.NewGuid(), "Pizza");

            var (margherita, _) = category.AddItem(
                Guid.NewGuid(),
                "Margherita",
                null,
                Money.FromPounds(9.99m));

            var (hawaiian, _) = category.AddItem(
                Guid.NewGuid(),
                "Hawaiian",
                null,
                Money.FromPounds(12.99m));

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(margherita.Id, 2);
            basket.AddItem(hawaiian.Id, 1);

            var mobileNumber = new MobileNumber("07123456789");

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(54, -2));

            var now = DateTimeOffset.UtcNow;

            var orderId = new OrderId(Guid.NewGuid().ToString());

            var (order, _) = restaurant.PlaceOrder(
                orderId,
                basket,
                menu,
                mobileNumber,
                deliveryLocation,
                billingAccount,
                now,
                TimeZoneInfo.Utc);

            order.Subtotal.ShouldBe(Money.FromPounds(2 * 9.99m + 12.99m));
        }
    }
}
