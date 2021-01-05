using System;

namespace Web.Domain.Restaurants
{
    public record RestaurantId(Guid Value) : GuidId(Value);
}
