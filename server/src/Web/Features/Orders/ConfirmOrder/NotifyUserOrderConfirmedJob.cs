using Web.Domain.Orders;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyUserOrderConfirmedJob : Job
    {
        public NotifyUserOrderConfirmedJob(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}
