using System.Threading;
using System.Threading.Tasks;
using Application.Services.Authentication;
using Domain;
using Domain.Restaurants;
namespace Application.Menus.UpdateMenuItem
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
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant owner can update the menu."));
            }

            if (!menu.ContainsCategory(command.CategoryName))
            {
                return Result.Fail(Error.NotFound($"Category {command.CategoryName} not found."));
            }

            var category = menu.GetCategory(command.CategoryName);

            if (!category.ContainsItem(command.OldItemName))
            {
                return Result.Fail(Error.NotFound($"Item {command.OldItemName} not found for category {command.CategoryName}."));
            }

            if (command.OldItemName != command.NewItemName && category.ContainsItem(command.NewItemName))
            {
                return Result.Fail(Error.BadRequest($"Item {command.NewItemName} already exists for category {command.CategoryName}."));
            }

            var item = category.GetItem(command.OldItemName);

            category.RenameItem(command.OldItemName, command.NewItemName);
            item.Description = command.Description;
            item.Price = new Money(command.Price);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
