using System;

namespace FoodSnap.Domain.Restaurants
{
    public class RestaurantId : ValueObject<RestaurantId>
    {
        public RestaurantId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Restaurant ID value is empty.");
            }

            Value = value;
        }

        public Guid Value { get; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool IsEqual(RestaurantId other)
        {
            return Value == other.Value;
        }
    }
}
