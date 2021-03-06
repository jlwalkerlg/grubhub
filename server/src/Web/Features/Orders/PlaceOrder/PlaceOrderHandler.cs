using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Features.Billing;
using Web.Services.DateTimeServices;
using Web.Services.Authentication;
using Web.Services.Geocoding;

namespace Web.Features.Orders.PlaceOrder
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IBillingService billingService;
        private readonly IGeocoder geocoder;

        public PlaceOrderHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider,
            IBillingService billingService,
            IGeocoder geocoder)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
            this.billingService = billingService;
            this.geocoder = geocoder;
        }

        public async Task<Result<string>> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork
                .Baskets
                .Get(authenticator.UserId, new RestaurantId(command.RestaurantId));

            if (basket is null) return Error.NotFound("Basket not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(basket.RestaurantId);

            if (!restaurant.HasBillingAccount())
            {
                return Error.NotFound("Restaurant not currently accepting orders.");
            }

            var (coordinates, lookupError) = await geocoder.LookupCoordinates(command.Postcode);

            if (lookupError) return Error.BadRequest("Address not recognised.");

            var billingAccount = await unitOfWork
                .BillingAccounts
                .GetById(restaurant.BillingAccountId);

            var deliveryLocation = new DeliveryLocation(
                new Address(
                    command.AddressLine1,
                    command.AddressLine2,
                    command.City,
                    new Postcode(command.Postcode)),
                coordinates);

            var menu = await unitOfWork
                .Menus
                .GetByRestaurantId(restaurant.Id);

            var (order, placeOrderError) = restaurant.PlaceOrder(
                new OrderId(Guid.NewGuid().ToString()),
                basket,
                menu,
                new MobileNumber(command.Mobile),
                deliveryLocation,
                billingAccount,
                dateTimeProvider.UtcNow,
                dateTimeProvider.BritishTimeZone);

            if (placeOrderError) return placeOrderError;

            var (paymentIntent, paymentIntentError) = await billingService.GeneratePaymentIntent(order, billingAccount);

            if (paymentIntentError) return Error.Internal("Failed to generate payment intent.");

            order.PaymentIntentId = paymentIntent.Id;
            order.PaymentIntentClientSecret = paymentIntent.ClientSecret;

            await unitOfWork.Orders.Add(order);
            await unitOfWork.Commit();

            return Result.Ok(order.Id.Value);
        }
    }
}
