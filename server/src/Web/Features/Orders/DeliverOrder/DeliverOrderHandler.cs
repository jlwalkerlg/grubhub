using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Orders.DeliverOrder
{
    public class DeliverOrderHandler : IRequestHandler<DeliverOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;

        public DeliverOrderHandler(
            IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(DeliverOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            if (restaurant.ManagerId != authenticator.UserId) return Error.Unauthorised();

            if (order.Delivered) return Result.Ok();

            var result = order.Deliver(dateTimeProvider.UtcNow);

            if (!result) return result.Error;

            await unitOfWork.Outbox.Add(new OrderDeliveredEvent(order.Id, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
