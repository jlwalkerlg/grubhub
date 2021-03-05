using System;

namespace Web.Features.Baskets.UpdateBasketItemQuantity
{
    public record UpdateBasketItemQuantityCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}
