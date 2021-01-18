using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryHandler : IRequestHandler<AddMenuCategoryCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public AddMenuCategoryHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddMenuCategoryCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant owner can add menu categories.");
            }

            if (menu.ContainsCategory(command.Name))
            {
                return Error.BadRequest($"Category {command.Name} already exists.");
            }

            menu.AddCategory(Guid.NewGuid(), command.Name);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
