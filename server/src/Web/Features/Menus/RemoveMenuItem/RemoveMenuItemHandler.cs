using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Menus.RemoveMenuItem
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
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

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
                return Error.BadRequest($"Category {command.CategoryName} not found.");
            }

            var category = menu.GetCategory(command.CategoryName);

            if (!category.ContainsItem(command.ItemName))
            {
                return Error.BadRequest($"Item {command.ItemName} not found for category {command.CategoryName}.");
            }

            category.RemoveItem(command.ItemName);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}