using System;
using Web.Domain.Orders;
using Web.Services.Events;

namespace Web.Features.Orders.AcceptOrder
{
    public record OrderAcceptedEvent : Event
    {
        public OrderAcceptedEvent(OrderId orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public OrderId OrderId { get; }
    }
}
