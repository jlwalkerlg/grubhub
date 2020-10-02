using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    public class AddMenuItemHandler : IRequestHandler<AddMenuItemCommand, Guid>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public AddMenuItemHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddMenuItemCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetById(command.MenuId);

            if (menu == null)
            {
                return Result<Guid>.Fail(Error.NotFound("Menu not found."));
            }

            if (!menu.Categories.Any(x => x.Id == command.CategoryId))
            {
                return Result<Guid>.Fail(Error.NotFound("Category not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result<Guid>.Fail(Error.Unauthorised("Only the restaurant owner can add menu items."));
            }

            menu.AddItem(
                command.CategoryId,
                command.Name,
                command.Description,
                new Money(command.Price));

            await unitOfWork.Commit();

            var item = menu
                .Categories.First(x => x.Id == command.CategoryId)
                .Items.Last(x => x.Name == command.Name);

            return Result.Ok(item.Id);
        }
    }
}
