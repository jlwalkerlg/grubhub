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

            if (!menu.ContainsCategory(command.CategoryName))
            {
                return Error.BadRequest($"Category {command.CategoryName} doesn't exist.");
            }

            var category = menu.GetCategory(command.CategoryName);

            if (!category.ContainsItem(command.OldItemName))
            {
                return Error.BadRequest($"Item {command.OldItemName} doesn't exist for category {command.CategoryName}.");
            }

            if (command.OldItemName != command.NewItemName
                && category.ContainsItem(command.NewItemName))
            {
                return Error.BadRequest($"Item {command.NewItemName} already exists for category {command.CategoryName}.");
            }

            var item = category.GetItem(command.OldItemName);

            category.RenameItem(command.OldItemName, command.NewItemName);

            item.Description = string.IsNullOrWhiteSpace(command.Description)
                ? null
                : command.Description;

            item.Price = new Money(command.Price);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
