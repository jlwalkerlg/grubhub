using FoodSnap.Application.Events;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Restaurants
{
    public class RestaurantApprovedEvent : Event
    {
        public RestaurantApprovedEvent(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
