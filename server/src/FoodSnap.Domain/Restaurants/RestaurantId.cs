using System;

namespace FoodSnap.Domain.Restaurants
{
    public class RestaurantId : GuidId
    {
        public RestaurantId(Guid value) : base(value)
        {
        }
    }
}
