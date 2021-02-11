using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Baskets.AddToBasket
{
    public class AddToBasketHandler : IRequestHandler<AddToBasketCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public AddToBasketHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddToBasketCommand command, CancellationToken cancellationToken)
        {
            var menu = await unitOfWork.Menus
                .GetByRestaurantId(new RestaurantId(command.RestaurantId));

            if (menu == null)
            {
                return Error.NotFound("Menu not found.");
            }

            if (!menu.ContainsItem(command.MenuItemId))
            {
                return Error.NotFound("Menu item not found.");
            }

            var basket = await unitOfWork.Baskets
                .Get(authenticator.UserId, menu.RestaurantId);

            if (basket == null)
            {
                basket = new Basket(
                    authenticator.UserId,
                    menu.RestaurantId);

                await unitOfWork.Baskets.Add(basket);
            }

            basket.AddItem(command.MenuItemId, command.Quantity);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
