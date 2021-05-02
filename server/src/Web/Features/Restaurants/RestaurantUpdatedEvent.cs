using System;
using Web.Services.Events;

namespace Web.Features.Restaurants
{
    public record RestaurantUpdatedEvent(Guid RestaurantId, DateTimeOffset OccuredAt) : Event(OccuredAt);
}
