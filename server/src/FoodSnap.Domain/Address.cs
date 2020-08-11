using System;

namespace FoodSnap.Domain
{
    public class Address : ValueObject<Address>
    {
        public string Line1 { get; }
        public string Line2 { get; }
        public string Town { get; }
        public Postcode Postcode { get; }

        public Address(string line1, string line2, string town, Postcode postcode)
        {
            if (string.IsNullOrWhiteSpace(line1))
            {
                throw new ArgumentException($"{nameof(line1)} must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(town))
            {
                throw new ArgumentException($"{nameof(town)} must not be empty.");
            }

            if (postcode is null)
            {
                throw new ArgumentNullException(nameof(postcode));
            }

            Line1 = line1;
            Line2 = line2;
            Town = town;
            Postcode = postcode;
        }

        // EF Core
        private Address() { }

        public override int GetHashCode()
        {
            var hashCode = Line1.GetHashCode();
            hashCode = (hashCode * 397) ^ Line2.GetHashCode();
            hashCode = (hashCode * 397) ^ Town.GetHashCode();
            hashCode = (hashCode * 397) ^ Postcode.GetHashCode();
            return hashCode;
        }

        protected override bool IsEqual(Address other)
        {
            return Line1 == other.Line1
                && Line2 == other.Line2
                && Town == other.Town
                && Postcode == other.Postcode;
        }
    }
}
