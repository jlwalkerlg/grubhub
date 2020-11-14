using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace FoodSnap.Application.Validation
{
    public static class ValidatorExtensions
    {
        private static Regex phoneNumberRegex = new(
            "^[0-9]{5} ?[0-9]{6}$",
            RegexOptions.Compiled,
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

        public static IRuleBuilderOptions<T, int> Min<T>(this IRuleBuilder<T, int> ruleBuilder, int min)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(min)
                .WithMessage($"Must be greater than or equal to {min}.");
        }

        public static IRuleBuilderOptions<T, float> Min<T>(this IRuleBuilder<T, float> ruleBuilder, float min)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(min)
                .WithMessage($"Must be greater than or equal to {min}.");
        }

        public static IRuleBuilderOptions<T, decimal> Min<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal min)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(min)
                .WithMessage($"Must be greater than or equal to {min}.");
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
    }
}
