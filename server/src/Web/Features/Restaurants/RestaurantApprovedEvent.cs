using System;
using Web.Services.Events;

namespace Web.Features.Restaurants
{
    public record RestaurantApprovedEvent(Guid RestaurantId, DateTimeOffset OccuredAt) : Event(OccuredAt);
}
