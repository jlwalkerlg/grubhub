using System;
using Web.Services.Events;

namespace Web.Features.Orders.DeliverOrder
{
    public record OrderDeliveredEvent : Event
    {
        public OrderDeliveredEvent(string orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
