using FoodSnap.Application.Events;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Restaurants
{
    public record RestaurantApprovedEvent(RestaurantId RestaurantId) : Event;
}
