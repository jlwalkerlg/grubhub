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

            var numbers = string.Join("", value.Where(x => x >= '0' && x <= '9'));

            if (numbers.StartsWith("07") && numbers.Length == 11)
            {
                return true;
            }

            if (numbers.StartsWith("447") && numbers.Length == 12)
            {
                return true;
            }

            return false;
        }
    }
}
