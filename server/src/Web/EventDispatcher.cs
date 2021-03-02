using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Features.Orders.AcceptOrder;
using Web.Features.Orders.ConfirmOrder;
using Web.Services.Events;

namespace Web
{
    public class EventDispatcher
    {
        private readonly ISender sender;

        public EventDispatcher(ISender sender)
        {
            this.sender = sender;
        }

        public async Task<Result> Dispatch(Event ev, CancellationToken cancellationToken)
        {
            return ev switch
            {
                OrderConfirmedEvent ocEv => await sender.Send(
                    new HandleEventCommand<OrderConfirmedEvent>(ocEv),
                    cancellationToken),

                OrderAcceptedEvent oaEv => await sender.Send(
                    new HandleEventCommand<OrderAcceptedEvent>(oaEv),
                    cancellationToken),

                _ => Result.Ok(),
            };
        }
    }
}
