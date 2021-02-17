using System.Threading;
using System.Threading.Tasks;
using Web.Features.Events;
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

        public async Task<Result> Handle(HandleEventCommand<OrderConfirmedEvent> command, CancellationToken cancellationToken)
        {
            var ev = command.Event;

            await queue.Enqueue(new NotifyUserOrderConfirmedJob(ev.OrderId));
            await queue.Enqueue(new NotifyRestaurantOrderConfirmedJob(ev.OrderId));

            return Result.Ok();
        }
    }
}
