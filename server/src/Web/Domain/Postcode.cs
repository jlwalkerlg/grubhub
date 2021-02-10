using System;
using System.Text.RegularExpressions;

namespace Web.Domain
{
    public record Postcode
    {
        private static readonly Regex regex = new(
            "^[A-Za-z]{2}[0-9]{1,2} ?[0-9]{1}[A-Za-z]{2}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public Postcode(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException("Postcode invalid.");
            }

            Value = value;
        }

        public string Value { get; }

        public static bool IsValid(string value)
        {
            return regex.IsMatch(value);
        }
    }
}
