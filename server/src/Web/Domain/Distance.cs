using System;

namespace Web.Domain
{
    public record Distance
    {
        private Distance() { } // EF

        private Distance(float km)
        {
            if (km < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(km));
            }

            Km = km;
        }

        public float Km { get; }

        public static Distance Zero => new Distance(0);

        public static Distance FromMetres(float metres) => new Distance(metres / 1000);

        public static Distance FromKm(float km) => new Distance(km);

        public static bool operator >(Distance a, Distance b) => a?.Km > b?.Km;

        public static bool operator <(Distance a, Distance b) => b > a;
    }
}
