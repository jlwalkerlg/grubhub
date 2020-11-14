using System;
using System.Text.RegularExpressions;

namespace FoodSnap.Domain
{
    public record Email
    {
        private static Regex regex = new(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Address must not be empty.");
            }

            if (!regex.IsMatch(address))
            {
                throw new ArgumentException("Address invalid.");
            }

            Address = address;
        }

        public string Address { get; }
    }
}
