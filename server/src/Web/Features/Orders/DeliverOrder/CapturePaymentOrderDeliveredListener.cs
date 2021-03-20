using System.Threading;
using System.Threading.Tasks;
using Stripe;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    public class CapturePaymentOrderDeliveredListener : IEventListener<OrderDeliveredEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly PaymentIntentService service = new();

        public CapturePaymentOrderDeliveredListener(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(OrderDeliveredEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await service.CaptureAsync(
                order.PaymentIntentId,
                new PaymentIntentCaptureOptions(),
                cancellationToken: cancellationToken);
        }
    }
}
