using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryHandler : IRequestHandler<RemoveMenuCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;

        public RemoveMenuCategoryHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(RemoveMenuCategoryCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus.GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Error.NotFound("Menu not found.");
            }

            var restaurant = await unitOfWork.Restaurants.GetById(new RestaurantId(command.RestaurantId));

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant manager can update the menu.");
            }

            var result = menu.RemoveCategory(command.CategoryId);

            if (result)
            {
                await unitOfWork.Publish(new MenuUpdatedEvent(menu.RestaurantId, dateTimeProvider.UtcNow));
                await unitOfWork.Commit();
            }

            return result;
        }
    }
}
