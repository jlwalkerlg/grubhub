using System;
using Web.Domain.Orders;
using Web.Services.Events;

namespace Web.Features.Orders.PlaceOrder
{
    public record OrderPlacedEvent : Event
    {
        public OrderPlacedEvent(OrderId orderId, DateTime createdAt) : base(createdAt)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}
