using System;

namespace Web.Domain
{
    public record Money
    {
        private Money(decimal pounds)
        {
            Pounds = pounds;
        }

        public decimal Pounds { get; }

        public int Pence => (int)(Pounds * 100);

        public static Money Zero => new Money(0);

        public static Money FromPence(int pence) => FromPounds((decimal)(pence) / 100);

        public static Money FromPounds(decimal pounds)
        {
            if (pounds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pounds));
            }

            var pence = (int)(pounds * 100);

            if ((decimal)pence != pounds * 100)
            {
                throw new ArgumentException("Money has minimum precision of 1p.");
            }

            return new Money(pounds);
        }

        public static bool operator <(Money a, Money b)
        {
            return a?.Pounds < b?.Pounds;
        }

        public static bool operator >(Money a, Money b) => b < a;

        public static Money operator +(Money a, Money b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Money(a.Pounds + b.Pounds);
        }

        public static Money operator *(Money money, int multiplier)
        {
            if (money is null)
            {
                throw new ArgumentNullException(nameof(money));
            }

            if (multiplier < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier));
            }

            return new Money(money.Pounds * multiplier);
        }

        public static Money operator *(int multiplier, Money money) => money * multiplier;
    }
}
