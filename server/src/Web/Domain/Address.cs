using System;
using System.Text;

namespace Web.Domain
{
    public record Address
    {
        private Address() { } // EF Core

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

            Line1 = line1;
            Line2 = line2;
            City = city;
            Postcode = postcode;
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string City { get; }
        public Postcode Postcode { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Line1);
            if (!string.IsNullOrWhiteSpace(Line2)) builder.Append($", {Line2}");
            builder.Append($", {City}");
            builder.Append($", {Postcode.Value}");

            return builder.ToString();
        }
    }
}
