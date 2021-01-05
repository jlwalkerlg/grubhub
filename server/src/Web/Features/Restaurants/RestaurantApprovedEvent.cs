using System;
using Web.Features.Events;
using Web.Domain.Restaurants;

namespace Web.Features.Restaurants
{
    public record RestaurantApprovedEvent(RestaurantId RestaurantId, DateTime CreatedAt) : Event(CreatedAt);
}
