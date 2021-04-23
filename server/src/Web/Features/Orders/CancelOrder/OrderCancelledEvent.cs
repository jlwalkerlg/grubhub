using System;
using Web.Services.Events;

namespace Web.Features.Orders.CancelOrder
{
    public record OrderCancelledEvent : Event
    {
        public OrderCancelledEvent(string orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
