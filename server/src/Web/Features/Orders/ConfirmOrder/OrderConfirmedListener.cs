using System.Threading;
using System.Threading.Tasks;
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

        public async Task Handle(OrderConfirmedEvent evnt, CancellationToken cancellationToken)
        {
            await queue.Enqueue(new Job[]
            {
                new NotifyUserOrderConfirmedJob(evnt.OrderId),
                new NotifyRestaurantOrderConfirmedJob(evnt.OrderId),
            },
            cancellationToken);
        }
    }
}
