using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Services;
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

            await queue.Enqueue(new Dictionary<Job, EnqueueOptions>()
            {
                {
                    new NotifyCustomerOrderDeliveredJob(
                        order.Id.Value,
                        order.UserId.Value.ToString()),
                    null
                },
                {
                    new NotifyRestaurantOrderDeliveredJob(
                        order.Id.Value,
                        order.RestaurantId.Value.ToString()),
                    null
                },
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
