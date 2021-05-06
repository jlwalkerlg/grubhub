using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Menus.AddMenuItem
{
    public class AddMenuItemHandler : IRequestHandler<AddMenuItemCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;

        public AddMenuItemHandler(
            IAuthenticator authenticator,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
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

            var category = menu.GetCategoryById(command.CategoryId);

            if (category == null)
            {
                return Error.NotFound("Item not found.");
            }

            var result = category.AddItem(
                Guid.NewGuid(),
                command.Name,
                command.Description,
                Money.FromPounds(command.Price));

            if (result)
            {
                await unitOfWork.Publish(new MenuUpdatedEvent(menu.RestaurantId, dateTimeProvider.UtcNow));
                await unitOfWork.Commit();
            }

            return result;
        }
    }
}
