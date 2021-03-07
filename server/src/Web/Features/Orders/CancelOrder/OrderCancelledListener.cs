using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Features.Orders.RejectOrder;
using Web.Services;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.CancelOrder
{
    public class OrderCancelledListener : IEventListener<OrderCancelledEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJobQueue queue;

        public OrderCancelledListener(IUnitOfWork unitOfWork, IJobQueue queue)
        {
            this.unitOfWork = unitOfWork;
            this.queue = queue;
        }

        public async Task<Result> Handle(OrderCancelledEvent evnt, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(evnt.OrderId));
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await queue.Enqueue(new Dictionary<Job, EnqueueOptions>()
            {
                {
                    new NotifyUserOrderCancelledJob(order.Id.Value, order.UserId.Value.ToString()),
                    null
                },
                {
                    new NotifyRestaurantOrderCancelledJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
                    null
                },
                {
                    new RefundOrderJob(order.PaymentIntentId),
                    null
                },
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
