using System.Threading;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Features.Orders.ConfirmOrder
{
    public class OrderConfirmedListener : IEventListener<OrderConfirmedEvent>
    {
        private readonly IJobQueue queue;

        public OrderConfirmedListener(IJobQueue queue)
        {
            this.queue = queue;
        }

        public async Task Handle(
            OrderConfirmedEvent ocEvent, CancellationToken cancellationToken)
        {
            await queue.Enqueue(new NotifyUserOrderConfirmedJob(ocEvent.OrderId));
            await queue.Enqueue(new NotifyRestaurantOrderConfirmedJob(ocEvent.OrderId));
        }
    }
}
