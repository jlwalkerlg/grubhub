using Web.Services.Jobs;

namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyUserOrderDeliveredJob : Job
    {
        public NotifyUserOrderDeliveredJob(string orderId, string userId)
        {
            OrderId = orderId;
            UserId = userId;
        }

        public string OrderId { get; }
        public string UserId { get; }
    }
}
