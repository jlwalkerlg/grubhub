using System;
using Web.Services.Events;

namespace Web.Features.Orders.RejectOrder
{
    public record OrderRejectedEvent : Event
    {
        public OrderRejectedEvent(string orderId, DateTime occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
