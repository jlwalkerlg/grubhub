using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Billing;
using Web.Services;
using Web.Services.Authentication;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;
        private readonly IClock clock;
        private readonly IBillingService billingService;

        public ConfirmOrderHandler(IUnitOfWork unitOfWork,
            IAuthenticator authenticator,
            IClock clock,
            IBillingService billingService)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
            this.clock = clock;
            this.billingService = billingService;
        }

        public async Task<Result> Handle(
            ConfirmOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders
                .GetById(new OrderId(command.OrderId));

            if (order == null)
            {
                return Error.NotFound("Order not found.");
            }

            if (authenticator.UserId != order.UserId)
            {
                return Error.Unauthorised();
            }

            var now = clock.UtcNow;

            var result = order.Confirm(now);

            if (!result)
            {
                return result.Error;
            }

            var billingServiceResult = await billingService.ConfirmOrder(order);

            if (!billingServiceResult)
            {
                return billingServiceResult.Error;
            }

            var ocEvent = new OrderConfirmedEvent(order.Id, now);

            await unitOfWork.Events.Add(ocEvent);
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
