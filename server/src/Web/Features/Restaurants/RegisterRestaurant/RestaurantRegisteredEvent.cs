using System;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Events;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public record RestaurantRegisteredEvent(RestaurantId RestaurantId, UserId ManagerId, DateTime CreatedAt) : Event(CreatedAt);
}
