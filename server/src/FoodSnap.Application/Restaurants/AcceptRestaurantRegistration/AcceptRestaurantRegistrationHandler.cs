using System.Threading;
using System.Threading.Tasks;

namespace FoodSnap.Application.Restaurants.AcceptRestaurantRegistration
{
    public class AcceptRestaurantRegistrationHandler : IRequestHandler<AcceptRestaurantRegistrationCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public AcceptRestaurantRegistrationHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AcceptRestaurantRegistrationCommand command,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetById(command.RestaurantId);

            if (restaurant is null)
            {
                return Result.Fail(Error.NotFound("Restaurant not found."));
            }

            restaurant.AcceptApplication();

            var @event = new RestaurantAcceptedEvent(restaurant.Id);
            await unitOfWork.Events.Add(@event);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
