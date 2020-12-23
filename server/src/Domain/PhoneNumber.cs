using System;
using System.Text.RegularExpressions;

namespace Domain
{
    public record PhoneNumber
    {
        private static Regex regex = new(
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

        public string Number { get; }
    }
}
