using System.Threading;
using System.Threading.Tasks;
using Web.Services.Authentication;
using Web.Domain;
using Web.Domain.Restaurants;

namespace Web.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsHandler : IRequestHandler<UpdateRestaurantDetailsCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateRestaurantDetailsHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(
            UpdateRestaurantDetailsCommand command,
            CancellationToken cancellationToken)
        {
            var id = new RestaurantId(command.RestaurantId);
            var restaurant = await unitOfWork.Restaurants.GetById(id);

            if (restaurant == null)
            {
                return Result.Fail(Error.NotFound("Restaurant not found."));
            }

            if (authenticator.UserId != restaurant.ManagerId)
            {
                return Result.Fail(Error.Unauthorised());
            }

            restaurant.Name = command.Name;
            restaurant.PhoneNumber = new PhoneNumber(command.PhoneNumber);
            restaurant.MinimumDeliverySpend = new Money(command.MinimumDeliverySpend);
            restaurant.DeliveryFee = new Money(command.DeliveryFee);
            restaurant.MaxDeliveryDistanceInKm = command.MaxDeliveryDistanceInKm;
            restaurant.EstimatedDeliveryTimeInMinutes = command.EstimatedDeliveryTimeInMinutes;

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
