using System;
using Web.Services.Events;

namespace Web.Features.Menus
{
    public record MenuUpdatedEvent(Guid RestaurantId, DateTimeOffset OccuredAt) : Event(OccuredAt);
}
