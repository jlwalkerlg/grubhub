using System.Threading;
using System.Threading.Tasks;
using Web.Features.Billing;
using Web.Services.Clocks;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClock clock;
        private readonly IBillingService billingService;

        public ConfirmOrderHandler(
            IUnitOfWork unitOfWork,
            IClock clock,
            IBillingService billingService)
        {
            this.unitOfWork = unitOfWork;
            this.clock = clock;
            this.billingService = billingService;
        }

        public async Task<Result> Handle(
            ConfirmOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders
                .GetByPaymentIntentId(command.PaymentIntentId);

            if (order is null)
            {
                return Error.NotFound("Order not found.");
            }

            if (order.AlreadyConfirmed)
            {
                return Result.Ok();
            }

            var now = clock.UtcNow;

            var accepted = await billingService.CheckPaymentWasAccepted(order);

            if (!accepted)
            {
                return Error.BadRequest("Payment not accepted.");
            }

            order.Confirm(clock.UtcNow);

            var basket = await unitOfWork.Baskets.Get(order.UserId, order.RestaurantId);

            if (basket != null)
            {
                await unitOfWork.Baskets.Remove(basket);
            }

            var ocEvent = new OrderConfirmedEvent(order.Id, now);

            await unitOfWork.Events.Add(ocEvent);
            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
