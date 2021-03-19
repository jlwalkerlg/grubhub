using System;
using Web.Services.Jobs;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyUserOrderRejectedJob : Job
    {
        public NotifyUserOrderRejectedJob(string orderId, string userId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        public string OrderId { get; }
        public string UserId { get; }
    }
}
