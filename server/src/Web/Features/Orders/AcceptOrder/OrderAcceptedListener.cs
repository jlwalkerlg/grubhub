using System;
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

        public async Task<Result> Handle(
            HandleEventCommand<OrderAcceptedEvent> command, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetById(command.Event.OrderId);
            var restaurant = await unitOfWork.Restaurants.GetById(order.RestaurantId);

            await queue.Enqueue(new Dictionary<Job, EnqueueOptions>()
            {
                {
                    new NotifyCustomerOrderAcceptedJob(order.Id.Value, order.UserId.Value.ToString()),
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
