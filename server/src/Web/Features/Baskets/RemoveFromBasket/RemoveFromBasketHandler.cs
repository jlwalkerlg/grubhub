using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketHandler : IRequestHandler<RemoveFromBasketCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public RemoveFromBasketHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveFromBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Baskets.Get(
                authenticator.UserId,
                new RestaurantId(command.RestaurantId));

            if (basket == null)
            {
                return Error.NotFound("Basket not found.");
            }

            var result = basket.RemoveItem(command.MenuItemId);

            if (!result)
            {
                return result.Error;
            }

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
