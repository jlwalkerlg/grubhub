using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Menus;
using FoodSnap.Shared;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemHandler : IRequestHandler<RemoveMenuItemCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public RemoveMenuItemHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveMenuItemCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetById(new MenuId(command.MenuId));

            if (menu == null)
            {
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant owner can update the menu."));
            }

            var category = menu.Categories.FirstOrDefault(x => x.Name == command.CategoryName);

            if (category == null)
            {
                return Result.Fail(Error.NotFound($"Category {command.CategoryName} not found."));
            }

            var item = category.Items.FirstOrDefault(x => x.Name == command.ItemName);

            if (item == null)
            {
                return Result.Fail(Error.NotFound($"Item {command.ItemName} not found for category {command.CategoryName}."));
            }

            category.RemoveItem(command.ItemName);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
