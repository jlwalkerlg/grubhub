using System;

namespace Web.Domain
{
    public record Distance
    {
        private readonly float metres;

        private Distance(float metres)
        {
            if (metres < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(metres));
            }

            this.metres = metres;
        }

        public float Km => metres / 1000;

        public static Distance FromMetres(float metres)
        {
            return new Distance(metres);
        }
    }
}
