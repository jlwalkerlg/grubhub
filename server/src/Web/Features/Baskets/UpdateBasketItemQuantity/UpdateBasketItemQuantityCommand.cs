using System;
using Web.Services.Authentication;

namespace Web.Features.Baskets.UpdateBasketItemQuantity
{
    [Authenticate]
    public record UpdateBasketItemQuantityCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}