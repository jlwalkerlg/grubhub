using System;
using Web.Services.Jobs;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyUserOrderCancelledJob : Job
    {
        public NotifyUserOrderCancelledJob(string orderId, string userId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        public string OrderId { get; }
        public string UserId { get; }
    }
}
