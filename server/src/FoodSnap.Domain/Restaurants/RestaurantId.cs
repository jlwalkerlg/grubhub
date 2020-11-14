using System;

namespace FoodSnap.Domain.Restaurants
{
    public record RestaurantId(Guid Value) : GuidId(Value);
}
