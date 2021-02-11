using System;
using System.Collections.Generic;

namespace Web.Features.Baskets
{
    public record BasketDto
    {
        public Guid UserId { get; init; }
        public Guid RestaurantId { get; init; }
        public List<BasketItemDto> Items { get; init; } = new();
    }

    public record BasketItemDto
    {
        public Guid MenuItemId { get; init; }
        public string MenuItemName { get; init; }
        public string MenuItemDescription { get; init; }
        public decimal MenuItemPrice { get; init; }
        public int Quantity { get; init; }
    }
}
