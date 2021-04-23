using System;
using Web.Domain.Restaurants;
using Web.Services.Events;

namespace Web.Features.Restaurants
{
    public record RestaurantApprovedEvent : Event
    {
        public RestaurantApprovedEvent(RestaurantId restaurantId, DateTimeOffset occuredAt) : base(occuredAt)
        {
            RestaurantId = restaurantId ?? throw new ArgumentNullException(nameof(restaurantId));
        }

        public RestaurantId RestaurantId { get; }
    }
}
