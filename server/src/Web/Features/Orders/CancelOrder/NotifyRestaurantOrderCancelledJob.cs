using System;
using Web.Services.Jobs;

namespace Web.Features.Orders.CancelOrder
{
    public class NotifyRestaurantOrderCancelledJob : Job
    {
        public NotifyRestaurantOrderCancelledJob(string orderId, string managerId)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
            ManagerId = managerId ?? throw new ArgumentNullException(nameof(managerId));
        }

        public string OrderId { get; }
        public string ManagerId { get; }
    }
}
