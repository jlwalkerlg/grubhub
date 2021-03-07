namespace Web.Features.Orders.DeliverOrder
{
    public class EmailUserOrderDeliveredJob : Job
    {
        public EmailUserOrderDeliveredJob(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; }
    }
}
