using System;
using Web.Domain.Orders;
using Web.Services.Events;

namespace Web.Features.Orders.ConfirmOrder
{
    public record OrderConfirmedEvent : Event
    {
        public OrderConfirmedEvent(OrderId orderId, DateTime CreatedAt) : base(CreatedAt)
        {
            OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        }

        public OrderId OrderId { get; }
    }
}
