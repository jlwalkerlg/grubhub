using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Restaurants;
namespace FoodSnap.Application.Menus.AddMenuCategory
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
                return Result.Fail(Error.NotFound("Menu not found."));
            }

            var restaurant = await unitOfWork.Restaurants.GetById(menu.RestaurantId);

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Result.Fail(Error.Unauthorised("Only the restaurant owner can add menu categories."));
            }

            if (menu.ContainsCategory(command.Name))
            {
                return Result.Fail(Error.BadRequest($"Category {command.Name} already exists."));
            }

            menu.AddCategory(command.Name);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
