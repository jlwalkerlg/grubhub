using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Shared;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    public class AddMenuItemHandler : IRequestHandler<AddMenuItemCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public AddMenuItemHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddMenuItemCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant owner can add menu items."));
            }

            if (!menu.ContainsCategory(command.CategoryName))
            {
                return Result.Fail(Error.NotFound($"Category {command.CategoryName} not found."));
            }

            var category = menu.GetCategory(command.CategoryName);

            if (category.ContainsItem(command.ItemName))
            {
                return Result.Fail(Error.BadRequest($"Item {command.ItemName} already exists for category {command.CategoryName}."));
            }

            category.AddItem(command.ItemName, command.Description, new Money(command.Price));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
