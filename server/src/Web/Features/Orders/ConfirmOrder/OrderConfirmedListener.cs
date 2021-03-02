using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Services;
using Web.Services.Events;
using Web.Services.Jobs;

namespace Web.Features.Orders.ConfirmOrder
{
    public class OrderConfirmedListener : IEventListener<OrderConfirmedEvent>
    {
        private readonly IJobQueue queue;

        public OrderConfirmedListener(IJobQueue queue)
        {
            this.queue = queue;
        }

        public async Task<Result> Handle(HandleEventCommand<OrderConfirmedEvent> command, CancellationToken cancellationToken)
        {
            var ev = command.Event;

            await queue.Enqueue(
                new Dictionary<Job, EnqueueOptions>()
                {
                    { new NotifyUserOrderConfirmedJob(ev.OrderId), null },
                    { new NotifyRestaurantOrderConfirmedJob(ev.OrderId), null },
                },
                cancellationToken);

            return Result.Ok();
        }
    }
}
