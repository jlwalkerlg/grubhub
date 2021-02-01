using System;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Services;
using Web.Services.Authentication;

namespace Web.Features.Orders.SubmitOrder
{
    public class SubmitOrderHandler : IRequestHandler<SubmitOrderCommand>
    {
        private readonly IAuthenticator authenticator;
        private readonly IUnitOfWork unitOfWork;
        private readonly DateTime now;

        public SubmitOrderHandler(IAuthenticator authenticator, IUnitOfWork unitOfWork, IClock clock)
        {
            this.authenticator = authenticator;
            this.unitOfWork = unitOfWork;

            now = clock.UtcNow;
        }

        public async Task<Result> Handle(SubmitOrderCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await unitOfWork.Restaurants
                .GetById(new RestaurantId(command.RestaurantId));

            if (restaurant == null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            var order = await unitOfWork.Orders
                .GetActiveOrder(authenticator.UserId, restaurant.Id);

            if (order == null)
            {
                return Error.NotFound("Order not found.");
            }

            var result = restaurant.SubmitOrder(order, now);

            if (result)
            {
                var ev = new OrderSubmittedEvent(order.Id, now);
                await unitOfWork.Events.Add(ev);

                await unitOfWork.Commit();
            }

            return result;
        }
    }
}
