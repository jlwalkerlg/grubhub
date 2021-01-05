using System;

namespace Web.Features.Menus.GetMenuByRestaurantId
{
    public record GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; init; }
    }
}
