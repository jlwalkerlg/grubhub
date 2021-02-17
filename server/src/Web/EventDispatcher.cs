using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Data.EF;
using Web.Features.Events;
using Web.Features.Orders.ConfirmOrder;

namespace Web
{
    public class EventDispatcher
    {
        private readonly ISender sender;

        public EventDispatcher(ISender sender)
        {
            this.sender = sender;
        }

        public async Task<Result> Dispatch(EventDto ev, CancellationToken cancellationToken)
        {
            var result = Result.Ok();

            if (ev.EventType == typeof(OrderConfirmedEvent).FullName)
            {
                result = await sender.Send(
                    new HandleEventCommand<OrderConfirmedEvent>(
                        ev.ToEvent<OrderConfirmedEvent>()),
                    cancellationToken);
            }

            return result;
        }
    }
}
