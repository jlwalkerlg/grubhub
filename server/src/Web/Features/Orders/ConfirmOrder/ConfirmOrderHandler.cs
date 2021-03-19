using System.Threading;
using System.Threading.Tasks;
using Web.Features.Billing;
using Web.Services.DateTimeServices;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IBillingService billingService;
        private readonly IEventBus bus;

        public ConfirmOrderHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IBillingService billingService,
            IEventBus bus)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.billingService = billingService;
            this.bus = bus;
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

            if (order.Confirmed)
            {
                return Result.Ok();
            }

            var now = dateTimeProvider.UtcNow;

            var accepted = await billingService.CheckPaymentWasAccepted(order);

            if (!accepted)
            {
                return Error.BadRequest("Payment not accepted.");
            }

            order.Confirm(dateTimeProvider.UtcNow);

            var basket = await unitOfWork.Baskets.Get(order.UserId, order.RestaurantId);

            if (basket != null)
            {
                await unitOfWork.Baskets.Remove(basket);
            }

            await bus.Publish(new OrderConfirmedEvent(order.Id, now));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
