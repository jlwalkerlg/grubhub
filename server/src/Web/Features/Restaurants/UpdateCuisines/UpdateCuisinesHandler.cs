using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesHandler : IRequestHandler<UpdateCuisinesCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateCuisinesHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
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
                return Result.Fail(Error.Unauthorised());
            }

            var cuisines = (await unitOfWork.Cuisines.All())
                .Where(x => command.Cuisines.Contains(x.Name));

            restaurant.SetCuisines(cuisines);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
