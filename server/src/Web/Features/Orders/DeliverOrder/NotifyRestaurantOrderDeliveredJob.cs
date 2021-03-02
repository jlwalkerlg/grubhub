namespace Web.Features.Orders.DeliverOrder
{
    public class NotifyRestaurantOrderDeliveredJob : Job
    {
        public NotifyRestaurantOrderDeliveredJob(string orderId, string restaurantId)
        {
            OrderId = orderId;
            RestaurantId = restaurantId;
        }

        public string OrderId { get; }
        public string RestaurantId { get; }
    }
}
