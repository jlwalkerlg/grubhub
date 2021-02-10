using System;

namespace Web.Domain
{
    public record Money
    {
        public static readonly Money Zero = new Money(0m);

        public Money(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must not be less than 0.");
            }

            if (amount.ToString().Contains('.'))
            {
                var precision = amount.ToString()
                    .Split('.')[1]
                    .Length;

                if (precision > 2)
                {
                    throw new ArgumentException("Amount must not be more precise than 1p.");
                }
            }

            Amount = amount;
        }

        public decimal Amount { get; }

        public static bool operator <(Money a, Money b)
        {
            return a?.Amount < b?.Amount;
        }

        public static bool operator >(Money a, Money b)
        {
            return a?.Amount > b?.Amount;
        }

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

            return new Money(a.Amount + b.Amount);
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

            return new Money(money.Amount * multiplier);
        }

        public static Money operator *(int multiplier, Money money)
        {
            return money * multiplier;
        }
    }
}
