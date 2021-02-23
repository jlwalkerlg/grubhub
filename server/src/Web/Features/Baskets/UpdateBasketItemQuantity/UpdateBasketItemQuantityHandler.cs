using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityHandler : IRequestHandler<UpdateBasketItemQuantityCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateBasketItemQuantityHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }
        
        public async Task<Result> Handle(UpdateBasketItemQuantityCommand command, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork
                    .Baskets
                    .Get(authenticator.UserId, new RestaurantId(command.RestaurantId));
            
            if (basket is null)
            {
                return Error.NotFound("Basket not found.");
            }

            var result = basket.UpdateQuantity(command.MenuItemId, command.Quantity);

            if (!result)
            {
                return result.Error;
            }

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}