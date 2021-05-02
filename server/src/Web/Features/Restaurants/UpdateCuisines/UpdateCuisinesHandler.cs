using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesHandler : IRequestHandler<UpdateCuisinesCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;

        public UpdateCuisinesHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(
            UpdateCuisinesCommand command,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork
                .Restaurants
                .GetById(new RestaurantId(command.RestaurantId));

            if (restaurant == null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised();
            }

            var cuisines = (await unitOfWork.Cuisines.All())
                .Where(x => command.Cuisines.Contains(x.Name));

            restaurant.SetCuisines(cuisines);

            await unitOfWork.Outbox.Add(new RestaurantUpdatedEvent(restaurant.Id, dateTimeProvider.UtcNow));
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
