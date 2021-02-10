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
                new Address("12 Maine Road"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var pizza = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var margherita = pizza.AddItem(
                Guid.NewGuid(), "Margherita", null, new Money(9.99m))
                .Value;

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(margherita.Id, 1);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            authenticatorSpy.SignIn(order.UserId);

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
                OrderId = order.Id,
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                AddressLine3 = "Madchester",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeSuccessful();

            result.Value.ShouldBe(paymentIntent.ClientSecret);

            order.Status.ShouldBe(OrderStatus.Placed);
            order.Address.Value.ShouldBe("12 Maine Road, Oldham, Madchester, Manchester, MN12 1NM");
            order.PlacedAt.ShouldBe(now);

            billingServiceSpy.PaymentIntentAmount.ShouldBe(
                margherita.Price + restaurant.DeliveryFee + new Money(config.ServiceCharge));
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
            var orderId = new OrderId(Guid.NewGuid());
            await SetupSuccess(orderId);

            geocoderSpy.Result = Error.BadRequest("Address not recognised");

            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = "12 Maine Road",
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
            var orderId = new OrderId(Guid.NewGuid());
            await SetupSuccess(orderId);

            unitOfWorkSpy.BillingAccountsRepositorySpy.Accounts.Clear();

            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = "12 Maine Road",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_Order_Is_Not_Found()
        {
            var orderId = new OrderId(Guid.NewGuid());
            await SetupSuccess(orderId);

            unitOfWorkSpy.OrderRepositorySpy.Orders.Clear();

            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = "12 Maine Road",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var orderId = new OrderId(Guid.NewGuid());
            await SetupSuccess(orderId);

            authenticatorSpy.SignIn(new UserId(Guid.NewGuid()));

            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = "12 Maine Road",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }

        [Fact]
        public async Task It_Fails_If_The_Billing_Service_Fails()
        {
            var orderId = new OrderId(Guid.NewGuid());
            await SetupSuccess(orderId);

            billingServiceSpy.PaymentIntentResult = Error.Internal("Payment failed.");

            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = "12 Maine Road",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Internal);
        }

        private async Task SetupSuccess(OrderId orderId)
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("12 Maine Road"),
                new Coordinates(54, -2)
            );

            restaurant.OpeningTimes = OpeningTimes.Always;

            var menu = new Menu(restaurant.Id);
            var pizza = menu.AddCategory(Guid.NewGuid(), "Pizza").Value;
            var margherita = pizza.AddItem(
                Guid.NewGuid(), "Margherita", null, new Money(9.99m))
                .Value;

            var order = new Order(
                orderId,
                new UserId(Guid.NewGuid()),
                restaurant.Id
            );

            order.AddItem(margherita.Id, 1);

            var billingAccount = new BillingAccount(
                new BillingAccountId(Guid.NewGuid().ToString()),
                restaurant.Id
            );

            billingAccount.Enable();

            await unitOfWorkSpy.Restaurants.Add(restaurant);
            await unitOfWorkSpy.Menus.Add(menu);
            await unitOfWorkSpy.Orders.Add(order);
            await unitOfWorkSpy.BillingAccounts.Add(billingAccount);

            authenticatorSpy.SignIn(order.UserId);

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
        }
    }
}
