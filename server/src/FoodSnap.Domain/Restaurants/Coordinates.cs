using System;

namespace FoodSnap.Domain.Restaurants
{
    public class Coordinates : ValueObject<Coordinates>
    {
        public float Latitude { get; }
        public float Longitude { get; }

        public Coordinates(float latitude, float longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException($"{nameof(latitude)} out of range.");
            }

            if (longitude < -180 || longitude > 80)
            {
                throw new ArgumentException($"{nameof(longitude)} out of range.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        protected override bool IsEqual(Coordinates other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            var hashCode = Latitude.GetHashCode();
            hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
            return hashCode;
        }
    }
}
