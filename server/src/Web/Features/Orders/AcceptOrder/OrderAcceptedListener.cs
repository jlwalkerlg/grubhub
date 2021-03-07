using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Services;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.AcceptOrder
{
    public class OrderAcceptedListener : IEventListener<OrderAcceptedEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJobQueue queue;

        public OrderAcceptedListener(IUnitOfWork unitOfWork, IJobQueue queue)
        {
            this.unitOfWork = unitOfWork;
            this.queue = queue;
        }

        public async Task<Result> Handle(OrderAcceptedEvent evnt, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(evnt.OrderId);

            if (order is null) return Error.NotFound("Order not found.");

            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            if (restaurant is null) return Error.NotFound("Restaurant not found.");

            await queue.Enqueue(new Dictionary<Job, EnqueueOptions>()
            {
                {
                    new NotifyUserOrderAcceptedJob(order.Id.Value, order.UserId.Value.ToString()),
                    null
                },
                {
                    new NotifyRestaurantOrderAcceptedJob(order.Id.Value, restaurant.ManagerId.Value.ToString()),
                    null
                },
            }, cancellationToken);

            return Result.Ok();
        }
    }
}
