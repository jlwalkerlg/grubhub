using System;

namespace FoodSnap.Application.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; set; }
    }
}
