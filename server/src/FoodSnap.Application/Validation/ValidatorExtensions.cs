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

        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithState(x => new RequiredFailure());
        }

        public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength)
        {
            return ruleBuilder
                .MinimumLength(minLength)
                .WithState(x => new MinLengthFailure(minLength));
        }

        public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .EmailAddress()
                .WithState(x => new EmailFailure());
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(phoneNumberRegex)
                .WithState(x => new PhoneNumberFailure());
        }

        public static IRuleBuilderOptions<T, string> Postcode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(postcodeRegex)
                .WithState(x => new PostcodeFailure());
        }
    }
}
