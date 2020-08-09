using System;
using System.Text.RegularExpressions;

namespace FoodSnap.Domain
{
    public class Postcode : ValueObject<Postcode>
    {
        public string Code { get; }

        private static Regex regex = new Regex(
            "^[A-Z]{2}[0-9]{1,2} ?[0-9][A-Z]{2}$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public Postcode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"{nameof(code)} must not be empty.");
            }

            if (!regex.IsMatch(code))
            {
                throw new ArgumentException($"{nameof(code)} not valid.");
            }

            Code = code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        protected override bool IsEqual(Postcode other)
        {
            return Code == other.Code;
        }
    }
}
