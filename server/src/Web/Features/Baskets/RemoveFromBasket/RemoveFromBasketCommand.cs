using System;
using Web.Services.Authentication;

namespace Web.Features.Baskets.RemoveFromBasket
{
    [Authenticate]
    public record RemoveFromBasketCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
    }
}
