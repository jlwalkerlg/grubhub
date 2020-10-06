using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Menus;
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
            var menu = await unitOfWork.Menus.GetById(new MenuId(command.MenuId));

            if (menu == null)
            {
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant owner can add menu items."));
            }

            var category = menu.Categories.FirstOrDefault(x => x.Name == command.Category);

            if (category == null)
            {
                return Result.Fail(Error.NotFound($"Category {command.Category} not found."));
            }

            if (category.Items.Any(x => x.Name == command.Name))
            {
                return Result.Fail(Error.BadRequest($"Item {command.Name} already exists for category {command.Category}."));
            }

            category.AddItem(command.Name, command.Description, new Money(command.Price));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
