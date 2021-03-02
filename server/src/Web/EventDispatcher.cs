using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web.Features.Orders.AcceptOrder;
using Web.Features.Orders.ConfirmOrder;
using Web.Features.Orders.DeliverOrder;
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

        public async Task<Result> Dispatch(Event evnt, CancellationToken cancellationToken)
        {
            return evnt switch
            {
                OrderConfirmedEvent ev => await sender.Send(
                    new HandleEventCommand<OrderConfirmedEvent>(ev),
                    cancellationToken),

                OrderAcceptedEvent ev => await sender.Send(
                    new HandleEventCommand<OrderAcceptedEvent>(ev),
                    cancellationToken),

                OrderDeliveredEvent ev => await sender.Send(
                    new HandleEventCommand<OrderDeliveredEvent>(ev),
                    cancellationToken),

                _ => Result.Ok(),
            };
        }
    }
}
