using System;
using System.Text.RegularExpressions;
using FoodSnap.Application.Validation;
using FluentValidation;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        private static Regex phoneNumberRegex = new Regex(
            "^[0-9]{5} ?[0-9]{6}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        private static Regex postcodeRegex = new Regex(
            "^[A-Z]{2}[0-9]{1,2} ?[0-9][A-Z]{2}$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public RegisterRestaurantValidator()
        {
            RuleFor(x => x.ManagerName)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.ManagerEmail)
                .NotEmpty().WithState(x => new RequiredFailure())
                .EmailAddress().WithState(x => new InvaildEmailFailure());

            RuleFor(x => x.ManagerPassword)
                .NotEmpty().WithState(x => new RequiredFailure())
                .MinimumLength(8).WithState(x => new MinLengthFailure(8));

            RuleFor(x => x.RestaurantName)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.RestaurantPhoneNumber)
                .NotEmpty().WithState(x => new RequiredFailure())
                .Matches(phoneNumberRegex).WithState(x => new PhoneNumberFailure());

            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.Town)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.Postcode)
                .NotEmpty().WithState(x => new RequiredFailure())
                .Matches(postcodeRegex).WithState(x => new PostcodeFailure());
        }
    }
}
