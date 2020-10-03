using System;

namespace FoodSnap.Domain.Users
{
    public class UserId : ValueObject<UserId>
    {
        public UserId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("User ID value is empty.");
            }

            Value = value;
        }

        public Guid Value { get; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool IsEqual(UserId other)
        {
            return Value == other.Value;
        }
    }
}
