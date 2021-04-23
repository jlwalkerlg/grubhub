using System;
using Web.Domain.Orders;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    public record OrderDeliveredEvent : Event
    {
        public OrderDeliveredEvent(OrderId orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public OrderId OrderId { get; }
    }
}
