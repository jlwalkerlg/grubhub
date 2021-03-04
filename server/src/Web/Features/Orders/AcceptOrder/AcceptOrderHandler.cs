using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.DateTimeServices;

namespace Web.Features.Orders.AcceptOrder
{
    public class AcceptOrderHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IDateTimeProvider dateTimeProvider;

        public AcceptOrderHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator, IDateTimeProvider dateTimeProvider)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.dateTimeProvider = dateTimeProvider;
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

            var ev = new OrderAcceptedEvent(order.Id, dateTimeProvider.UtcNow);

            await unitOfWork.Events.Add(ev);
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
