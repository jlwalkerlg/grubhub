using System;
using System.Linq;

namespace Web.Domain
{
    public record MobileNumber
    {
        public MobileNumber(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException("Mobile number invalid.");
            }

            Value = value;
        }

        public string Value { get; }

        public static bool IsValid(string value)
        {
            if (value is null) return false;

            value = value.Replace("+44", "0").Replace(" ", "");

            if (!long.TryParse(value, out var number)) return false;

            return number >= 07000000000 && number < 08000000000;
        }
    }
}
