using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Actions.Restaurants.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; set; }
    }
}
