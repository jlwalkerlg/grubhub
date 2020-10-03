using FoodSnap.Application.Events;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RestaurantRegisteredEvent : Event
    {
        public RestaurantRegisteredEvent(RestaurantId restaurantId, UserId managerId)
        {
            RestaurantId = restaurantId;
            ManagerId = managerId;
        }

        public RestaurantId RestaurantId { get; }
        public UserId ManagerId { get; }
    }
}
