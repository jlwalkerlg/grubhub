using System;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyCustomerOrderRejectedJob : Job
    {
        public NotifyCustomerOrderRejectedJob(string orderId, string customerId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        }

        public string OrderId { get; }
        public string CustomerId { get; }
    }
}
