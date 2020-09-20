using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Domain.Menus;

namespace FoodSnap.Application.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantHandler : IRequestHandler<ApproveRestaurantCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public ApproveRestaurantHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

            var menu = new Menu(restaurant.Id);
            await unitOfWork.Menus.Add(menu);

            var @event = new RestaurantApprovedEvent(restaurant.Id);
            await unitOfWork.Events.Add(@event);

            await unitOfWork.Commit();

            return Result.Ok();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
