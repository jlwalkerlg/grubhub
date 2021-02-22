using System;
using System.Linq;

namespace Web.Domain
{
    public record Address
    {
        public Address(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public Address(string line1, string line2, string city, Postcode postcode)
        {
            if (string.IsNullOrWhiteSpace(line1))
            {
                throw new ArgumentNullException(nameof(line1));
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentNullException(nameof(city));
            }

            if (postcode is null)
            {
                throw new ArgumentNullException(nameof(postcode));
            }

            Value = string.Join(", ",
                (new[] { line1, line2, city, postcode.Value })
                    .Where(x => x != null));
        }

        private Address() { } // EF Core

        public string Value { get; }
    }
}
