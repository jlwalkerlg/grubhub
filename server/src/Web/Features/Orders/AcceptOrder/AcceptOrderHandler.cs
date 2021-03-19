using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;
using Web.Services.Events;

namespace Web.Features.Orders.AcceptOrder
{
    public class AcceptOrderHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventBus bus;

        public AcceptOrderHandler(
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

        public async Task<Result> Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order.Accepted)
            {
                return Result.Ok();
            }

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null)
            {
                return Error.NotFound("Restaurant not found.");
            }

            if (restaurant.ManagerId != authenticator.UserId)
            {
                return Error.Unauthorised();
            }

            var result = order.Accept(dateTimeProvider.UtcNow);

            if (!result) return result.Error;

            await bus.Publish(new OrderAcceptedEvent(order.Id, dateTimeProvider.UtcNow));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
