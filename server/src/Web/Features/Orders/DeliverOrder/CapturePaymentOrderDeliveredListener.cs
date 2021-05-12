using System.Threading.Tasks;
using DotNetCore.CAP;
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

        [CapSubscribe(nameof(OrderDeliveredEvent), Group = nameof(CapturePaymentOrderDeliveredListener))]
        public async Task Handle(OrderDeliveredEvent @event)
        {
            var order = await uow.Orders.GetById(@event.OrderId);

            await service.CaptureAsync(order.PaymentIntentId, new PaymentIntentCaptureOptions());
        }
    }
}
