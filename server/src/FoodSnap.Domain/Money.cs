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

            Amount = amount;
        }

        public decimal Amount { get; }
    }
}
