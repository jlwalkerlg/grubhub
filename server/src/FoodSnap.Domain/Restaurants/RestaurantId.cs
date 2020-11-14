using System;

namespace FoodSnap.Domain.Restaurants
{
    public record RestaurantId(Guid value) : GuidId(value);
}
