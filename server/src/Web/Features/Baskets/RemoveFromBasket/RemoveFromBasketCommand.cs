using System;

namespace Web.Features.Baskets.RemoveFromBasket
{
    public record RemoveFromBasketCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
    }
}
