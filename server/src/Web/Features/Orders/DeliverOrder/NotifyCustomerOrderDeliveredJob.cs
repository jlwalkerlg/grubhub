namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyCustomerOrderDeliveredJob : Job
    {
        public NotifyCustomerOrderDeliveredJob(string orderId, string customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }

        public string OrderId { get; }
        public string CustomerId { get; }
    }
}
