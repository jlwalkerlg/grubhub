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

        public async Task<Result> Dispatch(Event ev, CancellationToken cancellationToken)
        {
            var result = Result.Ok();

            if (ev is OrderConfirmedEvent ocEv)
            {
                result = await sender.Send(
                    new HandleEventCommand<OrderConfirmedEvent>(ocEv),
                    cancellationToken);
            }

            return result;
        }
    }
}
