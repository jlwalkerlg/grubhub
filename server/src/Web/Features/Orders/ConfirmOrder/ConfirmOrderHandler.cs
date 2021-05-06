using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Billing;
using Web.Services.DateTimeServices;

namespace Web.Features.Orders.ConfirmOrder
{
    public abstract class ConfirmOrderHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IBillingService billingService;

        protected ConfirmOrderHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IBillingService billingService)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.billingService = billingService;
        }

        protected async Task<Result> Handle(Order order, CancellationToken cancellationToken)
        {
            if (order is null) return Error.NotFound("Order not found.");

            if (order.Confirmed) return Result.Ok();

            var accepted = await billingService.CheckPaymentWasAccepted(order);

            if (!accepted) return Error.BadRequest("Payment not accepted.");

            order.Confirm(dateTimeProvider.UtcNow);

            var basket = await unitOfWork.Baskets.Get(order.UserId, order.RestaurantId);

            if (basket != null) await unitOfWork.Baskets.Remove(basket);

            await unitOfWork.Publish(new OrderConfirmedEvent(order.Id, order.ConfirmedAt!.Value));

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
