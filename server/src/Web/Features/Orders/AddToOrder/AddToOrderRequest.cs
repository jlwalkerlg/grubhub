using System;

namespace Web.Features.Orders.AddToOrder
{
    public record AddToOrderRequest
    {
        public Guid MenuItemId { get; init; }
    }
}
