using System;
using Application.Events;
using Domain.Restaurants;
using Domain.Users;

namespace Application.Restaurants.RegisterRestaurant
{
    public record RestaurantRegisteredEvent(RestaurantId RestaurantId, UserId ManagerId, DateTime CreatedAt) : Event(CreatedAt);
}
