using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryHandler : IRequestHandler<AddMenuCategoryCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;

        public AddMenuCategoryHandler(
            IAuthenticator authenticator,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
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
                await unitOfWork.Publish(new MenuUpdatedEvent(menu.RestaurantId, dateTimeProvider.UtcNow));
                await unitOfWork.Commit();
            }

            return result;
        }
    }
}
