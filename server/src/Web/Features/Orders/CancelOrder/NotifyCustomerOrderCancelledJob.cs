using System;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyCustomerOrderCancelledJob : Job
    {
        public NotifyCustomerOrderCancelledJob(string orderId, string customerId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        }

        public string OrderId { get; }
        public string CustomerId { get; }
    }
}
