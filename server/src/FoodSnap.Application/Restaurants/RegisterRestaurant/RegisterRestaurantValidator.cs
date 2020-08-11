using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        public RegisterRestaurantValidator()
        {
            RuleFor(x => x.ManagerName)
                .Required();

            RuleFor(x => x.ManagerEmail)
                .Required()
                .Email();

            RuleFor(x => x.ManagerPassword)
                .Required()
                .MinLength(8);

            RuleFor(x => x.RestaurantName)
                .Required();

            RuleFor(x => x.RestaurantPhoneNumber)
                .Required()
                .PhoneNumber();

            RuleFor(x => x.AddressLine1)
                .Required();

            RuleFor(x => x.Town)
                .Required();

            RuleFor(x => x.Postcode)
                .Required()
                .Postcode();
        }
    }
}
