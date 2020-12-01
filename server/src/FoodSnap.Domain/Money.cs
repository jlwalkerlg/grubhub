using System;

namespace FoodSnap.Domain
{
    public record Money
    {
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
    }
}
