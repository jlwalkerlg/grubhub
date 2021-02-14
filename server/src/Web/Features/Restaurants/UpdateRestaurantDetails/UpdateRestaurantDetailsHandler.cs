using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

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
                return Error.NotFound("Restaurant not found.");
            }

            if (authenticator.UserId != restaurant.ManagerId)
            {
                return Error.Unauthorised();
            }

            restaurant.Name = command.Name;
            restaurant.Description = string.IsNullOrWhiteSpace(command.Description)
                ? null
                : command.Description;
            restaurant.PhoneNumber = new PhoneNumber(command.PhoneNumber);
            restaurant.MinimumDeliverySpend = Money.FromPounds(command.MinimumDeliverySpend);
            restaurant.DeliveryFee = Money.FromPounds(command.DeliveryFee);
            restaurant.MaxDeliveryDistance = Distance.FromKm(command.MaxDeliveryDistanceInKm);
            restaurant.EstimatedDeliveryTimeInMinutes = command.EstimatedDeliveryTimeInMinutes;

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
