using System;
using System.Collections.Generic;

namespace Web.Features.Orders.GetActiveOrder
{
    public record OrderDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid RestaurantId { get; init; }
        public string Status { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();
    }

    public record OrderItemDto
    {
        public Guid MenuItemId { get; init; }
        public string MenuItemName { get; init; }
        public string MenuItemDescription { get; init; }
        public decimal MenuItemPrice { get; init; }
        public int Quantity { get; init; }
    }
}
