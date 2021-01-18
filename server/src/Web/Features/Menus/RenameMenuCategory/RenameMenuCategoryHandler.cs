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

            // TODO: this logic belongs in the domain model.
            // Just return Result from the domain.
            if (!menu.ContainsCategoryById(command.CategoryId))
            {
                return Error.BadRequest("Category not found.");
            }

            var category = menu.GetCategoryById(command.CategoryId);

            if (category.Name != command.NewName)
            {
                if (menu.ContainsCategory(command.NewName))
                {
                    return Error.BadRequest("Category already exists.");
                }

                menu.RenameCategory(command.CategoryId, command.NewName);

                await unitOfWork.Commit();
            }

            return Result.Ok();
        }
    }
}
