using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Menus.RenameMenuCategory
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
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant manager can update the menu.");
            }

            if (!menu.ContainsCategory(command.OldName))
            {
                return Error.BadRequest($"Category {command.OldName} doesn't exist for this menu.");
            }

            if (command.OldName != command.NewName && menu.ContainsCategory(command.NewName))
            {
                return Error.BadRequest($"Category {command.NewName} already exists.");
            }

            menu.RenameCategory(command.OldName, command.NewName);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
