using FoodSnap.Application.Validation;
using FoodSnap.Application.Validation.Failures;
using FluentValidation;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        public RegisterRestaurantValidator()
        {
            RuleFor(x => x.ManagerName)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.ManagerEmail)
                .NotEmpty().WithState(x => new RequiredFailure())
                .EmailAddress().WithState(x => new EmailFailure());

            RuleFor(x => x.ManagerPassword)
                .NotEmpty().WithState(x => new RequiredFailure())
                .MinimumLength(8).WithState(x => new MinLengthFailure(8));

            RuleFor(x => x.RestaurantName)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.RestaurantPhoneNumber)
                .NotEmpty().WithState(x => new RequiredFailure())
                .PhoneNumber();

            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.Town)
                .NotEmpty().WithState(x => new RequiredFailure());

            RuleFor(x => x.Postcode)
                .NotEmpty().WithState(x => new RequiredFailure())
                .Postcode();
        }
    }
}
