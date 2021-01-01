using System;
using Application.Events;
using Domain.Restaurants;

namespace Application.Restaurants
{
    public record RestaurantApprovedEvent(RestaurantId RestaurantId, DateTime CreatedAt) : Event(CreatedAt);
}
