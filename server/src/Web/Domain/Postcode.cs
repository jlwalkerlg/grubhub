using System;
using System.Text.RegularExpressions;

namespace Web.Domain
{
    public record Postcode
    {
        private static readonly Regex Regex = new(
            "^([a-z]{1,2}[0-9]{1,2}|[a-z]{1,2}[0-9][a-z]) ?[0-9]{1}[a-z]{2}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
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
            return value is not null && Regex.IsMatch(value);
        }
    }
}
