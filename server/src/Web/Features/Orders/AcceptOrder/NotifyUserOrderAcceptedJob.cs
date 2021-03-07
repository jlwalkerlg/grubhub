namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyUserOrderAcceptedJob : Job
    {
        public NotifyUserOrderAcceptedJob(string orderId, string userId)
        {
            OrderId = orderId;
            UserId = userId;
        }

        public string OrderId { get; }
        public string UserId { get; }
    }
}
