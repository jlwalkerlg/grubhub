using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Data.EF;
using Web.Features.Orders.ConfirmOrder;

namespace Web
{
    public class EventDispatcher
    {
        private readonly IPublisher publisher;

        public EventDispatcher(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public async Task Dispatch(EventDto e, CancellationToken cancellationToken)
        {
            if (e.EventType == typeof(OrderConfirmedEvent).FullName)
            {
                await publisher.Publish(
                    e.ToEvent<OrderConfirmedEvent>(),
                    cancellationToken);
            }

            e.MarkHandled();
        }
    }
}
