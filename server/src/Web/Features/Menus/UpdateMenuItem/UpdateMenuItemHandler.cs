using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemHandler : IRequestHandler<UpdateMenuItemCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public UpdateMenuItemHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateMenuItemCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus
                .GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant owner can update the menu.");
            }

            var category = menu.GetCategoryById(command.CategoryId);

            if (category == null)
            {
                return Error.BadRequest("Category not found.");
            }

            var item = category.GetItemById(command.ItemId);

            if (item == null)
            {
                return Error.BadRequest("Item not found.");
            }

            var renameResult = category.RenameItem(command.ItemId, command.Name);

            if (!renameResult)
            {
                return renameResult;
            }

            item.Description = string.IsNullOrWhiteSpace(command.Description)
                ? null
                : command.Description;

            item.Price = new Money(command.Price);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
