using System;

namespace Web.Domain
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

        public Distance CalculateDistance(Coordinates destination)
        {
            var R = 6371e3; // metres
            var φ1 = (Latitude * Math.PI) / 180; // φ, λ in radians
            var φ2 = (destination.Latitude * Math.PI) / 180;
            var Δφ = ((destination.Latitude - Latitude) * Math.PI) / 180;
            var Δλ = ((destination.Longitude - Longitude) * Math.PI) / 180;

            var a =
                Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c;

            return Distance.FromMetres((float)d);
        }
    }
}
