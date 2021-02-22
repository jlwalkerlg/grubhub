using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Features.Billing;
using Web.Services;
using Web.Services.Authentication;
using Web.Services.Geocoding;

namespace Web.Features.Orders.PlaceOrder
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IClock clock;
        private readonly IBillingService billingService;
        private readonly IGeocoder geocoder;
        private readonly Config config;

        public PlaceOrderHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IClock clock,
            IBillingService billingService,
            IGeocoder geocoder,
            Config config)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.clock = clock;
            this.billingService = billingService;
            this.geocoder = geocoder;
            this.config = config;
        }

        public async Task<Result<string>> Handle(
            PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork
                .Baskets
                .Get(authenticator.UserId, new RestaurantId(command.RestaurantId));

            if (basket == null)
            {
                return Error.NotFound("Basket not found.");
            }

            var billingAccount = await unitOfWork
                .BillingAccounts
                .GetByRestaurantId(basket.RestaurantId);

            if (billingAccount == null)
            {
                return Error.NotFound("Restaurant not currently accepting orders.");
            }

            var coordinatesResult = await geocoder.LookupCoordinates(command.Postcode);

            if (!coordinatesResult)
            {
                return Error.BadRequest("Address not recognised.");
            }

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.City,
                    new Postcode(command.Postcode)
                ),
                coordinatesResult.Value
            );

            var restaurant = await unitOfWork
                .Restaurants
                .GetById(basket.RestaurantId);

            var menu = await unitOfWork
                .Menus
                .GetByRestaurantId(restaurant.Id);

            var orderResult = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber(command.Mobile),
                deliveryLocation,
                billingAccount,
                clock.UtcNow);

            if (!orderResult)
            {
                return orderResult.Error;
            }

            var order = orderResult.Value;

            var paymentIntentResult = await billingService
                .GeneratePaymentIntent(order, billingAccount);

            if (!paymentIntentResult)
            {
                return Error.Internal("Failed to generate payment intent.");
            }

            order.PaymentIntentId = paymentIntentResult.Value.Id;
            order.PaymentIntentClientSecret = paymentIntentResult.Value.ClientSecret;

            var opEvent = new OrderPlacedEvent(order.Id, clock.UtcNow);

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Events.Add(opEvent);
            await unitOfWork.Commit();

            return Result.Ok(order.Id.Value);
        }
    }
}
