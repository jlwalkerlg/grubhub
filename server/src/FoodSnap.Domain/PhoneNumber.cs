using System;
using System.Text.RegularExpressions;

namespace FoodSnap.Domain
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        public string Number { get; }

        private static Regex regex = new Regex(
            "^[0-9]{5} ?[0-9]{6}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public PhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException("Phone number must not be empty.");
            }

            if (!regex.IsMatch(number))
            {
                throw new ArgumentException("Phone number invalid.");
            }

            Number = number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        protected override bool IsEqual(PhoneNumber other)
        {
            return Number == other.Number;
        }
    }
}
