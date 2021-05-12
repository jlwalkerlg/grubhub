using System;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    public record OrderConfirmedEvent : Event
    {
        public OrderConfirmedEvent(string orderId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public string OrderId { get; }
    }
}
