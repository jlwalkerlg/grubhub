using System;
using Shouldly;
using System.Threading.Tasks;
using Web;
using Web.Features.Orders.PlaceOrder;
using Xunit;
using WebTests.Doubles;
using Web.Domain.Orders;
using Web.Domain.Users;
using Web.Domain.Restaurants;
using Web.Domain;
using System.Linq;
using Web.Domain.Menus;
using Web.Domain.Billing;
using Web.Services.Geocoding;
using Web.Features.Billing;
using Web.Domain.Baskets;

namespace WebTests.Features.Orders.PlaceOrder
{
    public class PlaceOrderHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly ClockStub clockStub;
        private readonly BillingServiceSpy billingServiceSpy;
        private readonly GeocoderSpy geocoderSpy;
        private readonly Config config;
        private readonly PlaceOrderHandler handler;

        public PlaceOrderHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            authenticatorSpy = new AuthenticatorSpy();

            clockStub = new ClockStub();

            billingServiceSpy = new BillingServiceSpy();

            geocoderSpy = new GeocoderSpy();

            config = new Config();

            handler = new PlaceOrderHandler(
                unitOfWorkSpy,
                authenticatorSpy,
                clockStub,
                billingServiceSpy,
                geocoderSpy,
                config
            );
        }

        [Fact]
        public async Task It_Places_The_Order()
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

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            await authenticatorSpy.SignIn(basket.UserId);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };
            billingServiceSpy.PaymentIntentResult = Result.Ok(paymentIntent);

            geocoderSpy.Result = Result.Ok(
                new GeocodingResult()
                {
                    FormattedAddress = "12 Maine Road, Manchester, UK, MN12 1NM",
                    Coordinates = new Coordinates(54, -2),
                }
            );

            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurant.Id,
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            unitOfWorkSpy.OrderRepositorySpy.Orders.ShouldHaveSingleItem();

            var order = unitOfWorkSpy.OrderRepositorySpy.Orders.Single();

            order.Id.Value.ShouldBe(result.Value);
            order.UserId.ShouldBe(basket.UserId);
            order.RestaurantId.ShouldBe(restaurant.Id);
            order.PlacedAt.ShouldBe(now);
            order.MobileNumber.Value.ShouldBe(command.Mobile);
            order.Address.Value.ShouldBe("12 Maine Road, Oldham, Manchester, MN12 1NM");
            order.PaymentIntentId.ShouldBe(paymentIntent.Id);
            order.PaymentIntentClientSecret.ShouldBe(paymentIntent.ClientSecret);

            billingServiceSpy.PaymentIntentAmount.ShouldBe(order.CalculateTotal());
            billingServiceSpy.PaymentIntentAccount.ShouldBe(billingAccount);

            var opEvent = unitOfWorkSpy.EventRepositorySpy.Events
                .OfType<OrderPlacedEvent>()
                .Single();

            opEvent.OrderId.ShouldBe(order.Id);
            opEvent.CreatedAt.ShouldBe(now);
        }

        [Fact]
        public async Task It_Fails_If_Geocoding_Fails()
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

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            await authenticatorSpy.SignIn(basket.UserId);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };
            billingServiceSpy.PaymentIntentResult = Result.Ok(paymentIntent);

            geocoderSpy.Result = Error.NotFound("Geocoding failed.");

            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurant.Id,
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }

        [Fact]
        public async Task It_Fails_If_The_Billing_Account_Is_Not_Found()
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

            var menu = new Menu(restaurant.Id);
            var menuItem = menu
                .AddCategory(Guid.NewGuid(), "Pizza").Value
                .AddItem(Guid.NewGuid(), "Margherita", null, Money.FromPounds(10m)).Value;

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Baskets.Add(basket);

            await authenticatorSpy.SignIn(basket.UserId);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };
            billingServiceSpy.PaymentIntentResult = Result.Ok(paymentIntent);

            geocoderSpy.Result = Result.Ok(
                new GeocodingResult()
                {
                    FormattedAddress = "12 Maine Road, Manchester, UK, MN12 1NM",
                    Coordinates = new Coordinates(54, -2),
                }
            );

            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurant.Id,
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Basket_Is_Not_Found()
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

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            await authenticatorSpy.SignIn(basket.UserId);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };
            billingServiceSpy.PaymentIntentResult = Result.Ok(paymentIntent);

            geocoderSpy.Result = Result.Ok(
                new GeocodingResult()
                {
                    FormattedAddress = "12 Maine Road, Manchester, UK, MN12 1NM",
                    Coordinates = new Coordinates(54, -2),
                }
            );

            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurant.Id,
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Billing_Service_Fails()
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

            var basket = new Basket(
                new UserId(Guid.NewGuid()),
                restaurant.Id);

            basket.AddItem(menuItem.Id, 1);

            var deliveryLocation = new DeliveryLocation(
                new Address("12 Maine Road, Manchester, UK, MN12 1NM"),
                new Coordinates(54, -2));

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Baskets.Add(basket);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            await authenticatorSpy.SignIn(basket.UserId);

            var now = DateTime.UtcNow;
            clockStub.UtcNow = now;

            billingServiceSpy.PaymentIntentResult = Error.Internal("Billing service failed.");

            geocoderSpy.Result = Result.Ok(
                new GeocodingResult()
                {
                    FormattedAddress = "12 Maine Road, Manchester, UK, MN12 1NM",
                    Coordinates = new Coordinates(54, -2),
                }
            );

            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurant.Id,
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Internal);
        }
    }
}
