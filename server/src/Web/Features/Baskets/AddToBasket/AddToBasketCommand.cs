using System;
using Web.Services.Authentication;

namespace Web.Features.Baskets.AddToBasket
{
    [Authenticate]
    public record AddToBasketCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}
