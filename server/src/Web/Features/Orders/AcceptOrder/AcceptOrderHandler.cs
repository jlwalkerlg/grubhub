using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Authentication;
using Web.Services.Clocks;

namespace Web.Features.Orders.AcceptOrder
{
    public class AcceptOrderHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IClock clock;

        public AcceptOrderHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator, IClock clock)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.clock = clock;
        }

        public async Task<Result> Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(command.OrderId));

            if (order.AlreadyAccepted)
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

            order.Accept(clock.UtcNow);

            var ev = new OrderAcceptedEvent(order.Id, clock.UtcNow);

            await unitOfWork.Events.Add(ev);
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
