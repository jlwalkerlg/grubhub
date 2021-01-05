using System;
using Web.Domain.Restaurants;
using Web.Features.Events;

namespace Web.Features.Restaurants
{
    public record RestaurantApprovedEvent(RestaurantId RestaurantId, DateTime CreatedAt) : Event(CreatedAt);
}
