using System.Threading;
using System.Threading.Tasks;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.DeliverOrder
{
    public class OrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJobQueue queue;

        public OrderDeliveredListener(IUnitOfWork unitOfWork, IJobQueue queue)
        {
            this.unitOfWork = unitOfWork;
            this.queue = queue;
        }

        public async Task<Result> Handle(OrderDeliveredEvent evnt, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(evnt.OrderId);

            if (order is null) return Error.NotFound("Order not found.");

            await queue.Enqueue(new Job[]
            {
                new NotifyUserOrderDeliveredJob(
                    order.Id.Value,
                    order.UserId.Value.ToString()),
                new NotifyRestaurantOrderDeliveredJob(
                    order.Id.Value,
                    order.RestaurantId.Value.ToString()),
                new CapturePaymentJob(order.PaymentIntentId),
                new EmailUserOrderDeliveredJob(order.Id.Value),
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
