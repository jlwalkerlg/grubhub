using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Menus;
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
            var restaurant = await unitOfWork
                .Restaurants
                .GetById(new RestaurantId(command.RestaurantId));

            if (restaurant == null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant owner can add menu categories.");
            }

            var menu = await unitOfWork
                .Menus
                .GetByRestaurantId(restaurant.Id);

            if (menu == null)
            {
                menu = new Menu(restaurant.Id);
                await unitOfWork.Menus.Add(menu);
            }

            var result = menu.AddCategory(Guid.NewGuid(), command.Name);

            if (result)
            {
                await unitOfWork.Commit();
            }

            return result;
        }
    }
}
