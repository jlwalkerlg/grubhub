using System;

namespace Domain.Restaurants
{
    public record RestaurantId(Guid Value) : GuidId(Value);
}
