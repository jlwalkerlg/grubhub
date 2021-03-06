using System;

namespace Web.Features.Orders
{
    public class RefundOrderJob : Job
    {
        public RefundOrderJob(string orderId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
