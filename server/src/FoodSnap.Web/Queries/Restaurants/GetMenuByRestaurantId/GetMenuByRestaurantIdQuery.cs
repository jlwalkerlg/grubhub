using System;
using FoodSnap.Application;

namespace FoodSnap.Web.Queries.Restaurants.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdQuery : IRequest<MenuDto>
    {
        public Guid RestaurantId { get; set; }
    }
}
