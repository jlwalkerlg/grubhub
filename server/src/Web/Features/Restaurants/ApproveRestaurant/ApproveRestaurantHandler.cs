using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.DateTimeServices;
using Web.Services.Events;

namespace Web.Features.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantHandler : IRequestHandler<ApproveRestaurantCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventBus bus;

        public ApproveRestaurantHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IEventBus bus)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.bus = bus;
        }

        public async Task<Result> Handle(
            ApproveRestaurantCommand command,
            CancellationToken cancellationToken)
        {
            var id = new RestaurantId(command.RestaurantId);
            var restaurant = await unitOfWork.Restaurants.GetById(id);

            if (restaurant is null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            restaurant.Approve();

            await bus.Publish(new RestaurantApprovedEvent(restaurant.Id, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
