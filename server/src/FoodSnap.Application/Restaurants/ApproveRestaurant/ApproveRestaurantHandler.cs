using System.Threading;
using System.Threading.Tasks;

namespace FoodSnap.Application.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantHandler : IRequestHandler<ApproveRestaurantCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public ApproveRestaurantHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ApproveRestaurantCommand command,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetById(command.RestaurantId);

            if (restaurant is null)
            {
                return Result.Fail(Error.NotFound("Restaurant not found."));
            }

            restaurant.Approve();

            var @event = new RestaurantApprovedEvent(restaurant.Id);
            await unitOfWork.Events.Add(@event);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
