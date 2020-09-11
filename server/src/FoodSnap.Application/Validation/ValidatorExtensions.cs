using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace FoodSnap.Application.Validation
{
    public static class ValidatorExtensions
    {
        private static Regex phoneNumberRegex = new Regex(
            "^[0-9]{5} ?[0-9]{6}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        private static Regex postcodeRegex = new Regex(
            "^[A-Z]{2}[0-9]{1,2} ?[0-9][A-Z]{2}$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Must not be empty.");
        }

        public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength)
        {
            return ruleBuilder
                .MinimumLength(minLength)
                .WithMessage($"Must be at least {minLength} characters long.");
        }

        public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .EmailAddress()
                .WithMessage("Must be a valid email.");
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(phoneNumberRegex)
                .WithMessage("Must be a valid phone number.");
        }

        public static IRuleBuilderOptions<T, string> Postcode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(postcodeRegex)
                .WithMessage("Must be a valid postcode.");
        }
    }
}
