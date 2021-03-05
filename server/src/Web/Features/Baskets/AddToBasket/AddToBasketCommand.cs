using System;

namespace Web.Features.Baskets.AddToBasket
{
    public record AddToBasketCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}
