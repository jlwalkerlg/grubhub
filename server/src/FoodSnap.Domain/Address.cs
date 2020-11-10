using System;

namespace FoodSnap.Domain
{
    public class Address : ValueObject<Address>
    {
        public string Value { get; }

        public Address(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Address must not be empty.");
            }

            Value = value;
        }

        // EF Core
        private Address() { }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool IsEqual(Address other)
        {
            return Value == other.Value;
        }
    }
}
