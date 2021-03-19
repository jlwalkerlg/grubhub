using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    public class RejectOrderHandler : IRequestHandler<RejectOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventBus bus;

        public RejectOrderHandler(
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

        public async Task<Result> Handle(RejectOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised("Only the restaurant manager can reject orders.");
            }

            if (order.Rejected) return Result.Ok();

            var result = order.Reject(dateTimeProvider.UtcNow);

            if (!result) return result.Error;

            await bus.Publish(new OrderRejectedEvent(order.Id.Value, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
