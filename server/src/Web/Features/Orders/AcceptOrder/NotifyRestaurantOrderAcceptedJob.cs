namespace Web.Features.Orders.AcceptOrder
{
    public class NotifyRestaurantOrderAcceptedJob : Job
    {
        public NotifyRestaurantOrderAcceptedJob(string orderId, string managerId)
        {
            OrderId = orderId;
            ManagerId = managerId;
        }

        public string OrderId { get; }
        public string ManagerId { get; }
    }
}
