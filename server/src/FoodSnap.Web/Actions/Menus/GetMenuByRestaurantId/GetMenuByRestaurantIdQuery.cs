using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Actions.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; set; }
    }
}
