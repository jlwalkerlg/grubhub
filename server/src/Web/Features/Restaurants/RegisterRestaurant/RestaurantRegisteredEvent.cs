using System;
using Web.Features.Events;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public record RestaurantRegisteredEvent(RestaurantId RestaurantId, UserId ManagerId, DateTime CreatedAt) : Event(CreatedAt);
}
