using FluentValidation;

namespace Web.Services.Validation
{
    public static class ValidatorExtensions
    {
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

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
        {
            return ruleBuilder
                .MaximumLength(maxLength)
                .WithMessage($"Must not be greater that {maxLength} characters long.");
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
                .Must(Web.Domain.PhoneNumber.IsValid)
                .WithMessage("Must be a valid phone number.");
        }

        public static IRuleBuilderOptions<T, string> MobileNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(Web.Domain.MobileNumber.IsValid)
                .WithMessage("Must be a valid mobile number.");
        }

        public static IRuleBuilderOptions<T, string> Postcode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(Web.Domain.Postcode.IsValid)
                .WithMessage("Must be a valid postcode.");
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.MinLength(8);
        }
    }
}
