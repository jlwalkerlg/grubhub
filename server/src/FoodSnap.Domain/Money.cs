using System;

namespace FoodSnap.Domain
{
    public class Money : ValueObject<Money>
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

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        protected override bool IsEqual(Money other)
        {
            return Amount == other.Amount;
        }
    }
}
