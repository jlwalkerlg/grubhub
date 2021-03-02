using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.Clocks;

namespace Web.Features.Orders.DeliverOrder
{
    public class DeliverOrderHandler : IRequestHandler<DeliverOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IClock clock;

        public DeliverOrderHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator, IClock clock)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.clock = clock;
        }

        public async Task<Result> Handle(DeliverOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            if (restaurant.ManagerId != authenticator.UserId) return Error.Unauthorised();

            if (order.AlreadyDelivered) return Result.Ok();

            var result = order.Deliver(clock.UtcNow);

            if (!result) return result.Error;

            var evnt = new OrderDeliveredEvent(order.Id, clock.UtcNow);

            await unitOfWork.Events.Add(evnt);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
