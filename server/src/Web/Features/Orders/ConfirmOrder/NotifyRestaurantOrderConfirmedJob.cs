using Web.Domain.Orders;

namespace Web.Features.Orders.ConfirmOrder
{
    public class NotifyRestaurantOrderConfirmedJob : Job
    {
        public NotifyRestaurantOrderConfirmedJob(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}
