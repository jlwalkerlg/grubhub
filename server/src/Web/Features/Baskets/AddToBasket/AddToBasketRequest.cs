using System;

namespace Web.Features.Baskets.AddToBasket
{
    public record AddToBasketRequest
    {
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}
