using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain;

namespace FoodSnap.Application.Restaurants.UpdateRestaurantDetails
{
    [Authenticate]
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
            var restaurant = await unitOfWork.Restaurants.GetById(command.Id);

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

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
