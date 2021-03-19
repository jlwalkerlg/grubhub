using Web.Domain.Orders;
using Web.Services.Jobs;

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
