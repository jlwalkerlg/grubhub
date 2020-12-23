using Application.Events;
using Domain.Restaurants;

namespace Application.Restaurants
{
    public record RestaurantApprovedEvent(RestaurantId RestaurantId) : Event;
}
