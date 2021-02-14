using System;

namespace Web.Domain
{
    public record Money
    {
        private Money(int pence)
        {
            if (pence < 0)
            {
                throw new ArgumentException("Pence must not be less than 0.");
            }

            Pence = pence;
        }

        public int Pence { get; }

        public static Money Zero => new Money(0);

        public static Money FromPence(int pence) => new Money(pence);

        public static Money FromPounds(decimal pounds)
        {
            var pence = (int)(pounds * 100);

            if ((decimal)pence != pounds * 100)
            {
                throw new ArgumentException("Money has minimum precision of 1p.");
            }

            return new Money(pence);
        }

        public static bool operator <(Money a, Money b)
        {
            return a?.Pence < b?.Pence;
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

            return new Money(a.Pence + b.Pence);
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

            return new Money(money.Pence * multiplier);
        }

        public static Money operator *(int multiplier, Money money) => money * multiplier;
    }
}
