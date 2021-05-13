using System.Threading.Tasks;
using DotNetCore.CAP;
using Stripe;
using Web.Domain.Orders;
using Web.Features.Orders.CancelOrder;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    public class RefundPaymentOrderRejectedListener : IEventListener<OrderCancelledEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly PaymentIntentService service = new();

        public RefundPaymentOrderRejectedListener(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [Subscribe(nameof(OrderCancelledEvent), typeof(RefundPaymentOrderRejectedListener))]
        public async Task Handle(OrderCancelledEvent @event)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));

            await service.CancelAsync(order.PaymentIntentId, new PaymentIntentCancelOptions());
        }
    }
}
