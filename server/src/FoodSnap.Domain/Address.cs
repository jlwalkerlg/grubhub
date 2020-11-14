using System;

namespace FoodSnap.Domain
{
    public record Address
    {
        public Address(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Address must not be empty.");
            }

            Value = value;
        }

        public string Value { get; }

        private Address() { }
        // EF Core
    }
}
