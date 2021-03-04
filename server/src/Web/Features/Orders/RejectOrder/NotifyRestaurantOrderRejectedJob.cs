using System;

namespace Web.Features.Orders.RejectOrder
{
    public class NotifyRestaurantOrderRejectedJob : Job
    {
        public NotifyRestaurantOrderRejectedJob(string orderId, string managerId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            ManagerId = managerId ?? throw new ArgumentNullException(nameof(managerId));
        }

        public string OrderId { get; }
        public string ManagerId { get; }
    }
}
