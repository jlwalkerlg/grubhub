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
            var order = await unitOfWork
                .Orders
                .GetById(new OrderId(command.OrderId));

            if (order == null)
            {
                return Error.NotFound("Order not found.");
            }

            if (order.UserId != authenticator.UserId)
            {
                return Error.Unauthorised();
            }

            var billingAccount = await unitOfWork
                .BillingAccounts
                .GetByRestaurantId(order.RestaurantId);

            if (billingAccount == null)
            {
                return Error.NotFound("Restaurant not currently accepting orders.");
            }

            var address = new AddressDetails(
                command.AddressLine1,
                command.AddressLine2,
                command.AddressLine3,
                command.City,
                command.Postcode
            );

            var geocodingResult = await geocoder.Geocode(address);

            if (!geocodingResult)
            {
                return Error.BadRequest("Address not recognised.");
            }

            var deliveryLocation = new DeliveryLocation(
                address.ToAddress(),
                geocodingResult.Value.Coordinates
            );

            var restaurant = await unitOfWork
                .Restaurants
                .GetById(order.RestaurantId);

            var menu = await unitOfWork
                .Menus
                .GetByRestaurantId(restaurant.Id);

            var subtotal = menu.CalculateSubtotal(order);

            var result = restaurant.PlaceOrder(
                subtotal,
                order,
                deliveryLocation,
                billingAccount,
                clock.UtcNow);

            if (!result)
            {
                return result.Error;
            }

            var amount = subtotal
                + restaurant.DeliveryFee
                + new Money(config.ServiceCharge);

            var paymentResult = await billingService
                .GeneratePaymentIntent(amount, billingAccount);

            if (!paymentResult)
            {
                return Error.Internal("Failed to generate payment intent.");
            }

            var opEvent = new OrderPlacedEvent(order.Id, clock.UtcNow);

            await unitOfWork.Events.Add(opEvent);
            await unitOfWork.Commit();

            return Result.Ok(paymentResult.Value);
        }
    }
}
