using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;
using Web.Services.Events;

namespace Web.Features.Orders.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventBus bus;

        public CancelOrderHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider,
            IEventBus bus)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
            this.bus = bus;
        }

        public async Task<Result> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            if (restaurant.ManagerId != authenticator.UserId)
                return Error.Unauthorised("Only the restaurant manager can cancel orders.");

            if (order.Cancelled) return Result.Ok();

            var result = order.Cancel(dateTimeProvider.UtcNow);

            if (!result) return result.Error;

            await bus.Publish(new OrderCancelledEvent(order.Id.Value, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
