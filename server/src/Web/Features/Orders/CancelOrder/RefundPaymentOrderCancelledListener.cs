using System.Threading;
using System.Threading.Tasks;
using Stripe;
using Web.Domain.Orders;
using Web.Services.Events;

namespace Web.Features.Orders.CancelOrder
{
    public class RefundPaymentOrderCancelledListener : IEventListener<OrderCancelledEvent>
    {
        private readonly IUnitOfWork uow;
        private readonly PaymentIntentService service = new();

        public RefundPaymentOrderCancelledListener(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(OrderCancelledEvent @event, CancellationToken cancellationToken)
        {
            var order = await uow.Orders.GetById(new OrderId(@event.OrderId));

            await service.CancelAsync(
                order.PaymentIntentId,
                new PaymentIntentCancelOptions(),
                cancellationToken: cancellationToken);
        }
    }
}
