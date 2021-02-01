using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Domain.Restaurants;
using Web.Services.Authentication;

namespace Web.Features.Orders.AddToOrder
{
    public class AddToOrderHandler : IRequestHandler<AddToOrderCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;

        public AddToOrderHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddToOrderCommand command, CancellationToken cancellationToken)
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

            var order = await unitOfWork.Orders.GetActiveOrder(authenticator.UserId, menu.RestaurantId);

            if (order == null)
            {
                order = new Order(
                    new OrderId(Guid.NewGuid()),
                    authenticator.UserId,
                    menu.RestaurantId);

                await unitOfWork.Orders.Add(order);
            }

            order.AddItem(command.MenuItemId, command.Quantity);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
