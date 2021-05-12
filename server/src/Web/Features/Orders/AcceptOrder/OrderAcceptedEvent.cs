using System;
using Web.Services.Events;

namespace Web.Features.Orders.AcceptOrder
{
    public record OrderAcceptedEvent : Event
    {
        public OrderAcceptedEvent(string orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
