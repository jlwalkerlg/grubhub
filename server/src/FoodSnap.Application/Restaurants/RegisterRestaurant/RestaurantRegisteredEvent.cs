using FoodSnap.Application.Events;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public record RestaurantRegisteredEvent(RestaurantId RestaurantId, UserId ManagerId) : Event;
}
