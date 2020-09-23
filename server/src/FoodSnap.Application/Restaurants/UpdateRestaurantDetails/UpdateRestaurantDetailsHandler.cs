using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Domain;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsHandler : IRequestHandler<UpdateRestaurantDetailsCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateRestaurantDetailsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateRestaurantDetailsCommand command,
            CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants.GetById(command.Id);

            if (restaurant == null)
            {
                return Result.Fail(Error.NotFound("Restaurant not found."));
            }

            restaurant.Name = command.Name;
            restaurant.PhoneNumber = new PhoneNumber(command.PhoneNumber);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
