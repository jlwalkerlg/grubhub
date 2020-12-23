using System;

namespace Domain
{
    public record Coordinates
    {
        public Coordinates(float latitude, float longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("Latitude out of range.");
            }

            if (longitude < -180 || longitude > 80)
            {
                throw new ArgumentException("Longitude out of range.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; }
        public float Longitude { get; }
    }
}
