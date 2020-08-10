using System;
using System.Text.RegularExpressions;
using FluentValidation;
using FoodSnap.Application.Validation.Failures;

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

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => phoneNumberRegex.IsMatch(x))
                .WithState(x => new PhoneNumberFailure());
        }

        public static IRuleBuilderOptions<T, string> Postcode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => postcodeRegex.IsMatch(x))
                .WithState(x => new PostcodeFailure());
        }
    }
}
