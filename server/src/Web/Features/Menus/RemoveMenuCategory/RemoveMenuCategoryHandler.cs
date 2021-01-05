using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
namespace Web.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryHandler : IRequestHandler<RemoveMenuCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public RemoveMenuCategoryHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(RemoveMenuCategoryCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(new RestaurantId(command.RestaurantId));

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant manager can update the menu.");
            }

            if (!menu.ContainsCategory(command.CategoryName))
            {
                return Error.NotFound($"Category {command.CategoryName} doesn't exist for this menu.");
            }

            menu.RemoveCategory(command.CategoryName);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
