using System;
using System.Linq;
using Web.Domain;

namespace Web.Services.Geocoding
{
    public record AddressDetails
    {
        public AddressDetails(
            string line1,
            string line2,
            string line3,
            string city,
            string postcode)
        {
            if (string.IsNullOrWhiteSpace(line1))
            {
                throw new ArgumentException("Line 1 must not be empty.");
            }

            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentException("City must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(postcode))
            {
                throw new ArgumentException("Postcode must not be empty.");
            }

            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            Postcode = postcode;
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string Line3 { get; }
        public string City { get; }
        public string Postcode { get; }

        public Address ToAddress()
        {
            return new Address(
                string.Join(", ",
                    new[] { Line1, Line2, Line3, City, Postcode }
                        .Where(x => !string.IsNullOrWhiteSpace(x)))
            );
        }
    }
}
