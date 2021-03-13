using System.Threading;
using System.Threading.Tasks;
using Web.Domain.Orders;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.RejectOrder
{
    public class OrderRejectedListener : IEventListener<OrderRejectedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJobQueue queue;

        public OrderRejectedListener(IUnitOfWork unitOfWork, IJobQueue queue)
        {
            this.unitOfWork = unitOfWork;
            this.queue = queue;
        }

        public async Task<Result> Handle(OrderRejectedEvent evnt, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(new OrderId(evnt.OrderId));
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await queue.Enqueue(new Job[]
            {
                new NotifyUserOrderRejectedJob(order.Id.Value, order.UserId.Value.ToString()),
                new NotifyRestaurantOrderRejectedJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
                new RefundOrderJob(order.PaymentIntentId),
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
