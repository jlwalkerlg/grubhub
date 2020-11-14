using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Application;

namespace FoodSnap.Application.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryHandler : IRequestHandler<RenameMenuCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public RenameMenuCategoryHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(RenameMenuCategoryCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant manager can update the menu."));
            }

            if (!menu.ContainsCategory(command.OldName))
            {
                return Result.Fail(Error.NotFound($"Category {command.OldName} doesn't exist for this menu."));
            }

            if (command.OldName != command.NewName && menu.ContainsCategory(command.NewName))
            {
                return Result.Fail(Error.BadRequest($"Category {command.NewName} already exists."));
            }

            menu.RenameCategory(command.OldName, command.NewName);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
