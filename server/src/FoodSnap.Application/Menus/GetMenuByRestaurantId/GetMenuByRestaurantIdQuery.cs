using System;

namespace FoodSnap.Application.Menus.GetMenuByRestaurantId
{
    public record GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; init; }
    }
}
