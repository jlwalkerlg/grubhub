using System.Threading.Tasks;
using MediatR;
using Web.Data.EF;
using Web.Features.Orders.ConfirmOrder;

namespace Web
{
    public class EventProcessor
    {
        private readonly IPublisher publisher;

        public EventProcessor(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public Task ProcessEvent(EventDto e)
        {
            if (e.EventType == typeof(OrderConfirmedEvent).FullName)
            {
                e.Handled = true;
                return publisher.Publish(e.ToEvent<OrderConfirmedEvent>());
            }

            return Task.CompletedTask;
        }
    }
}
