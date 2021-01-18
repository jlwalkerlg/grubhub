using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Menus.AddMenuItem
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
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant owner can add menu items.");
            }

            if (!menu.ContainsCategoryById(command.CategoryId))
            {
                return Error.NotFound("Category not found.");
            }

            var category = menu.GetCategory(command.CategoryId);

            if (category.ContainsItem(command.Name))
            {
                return Error.BadRequest("Item already exists.");
            }

            category.AddItem(
                Guid.NewGuid(),
                command.Name,
                command.Description,
                new Money(command.Price));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
