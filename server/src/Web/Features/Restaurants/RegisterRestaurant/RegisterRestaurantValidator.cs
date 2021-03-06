using Web.Services.Validation;

namespace Web.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantValidator : FluentValidator<RegisterRestaurantCommand>
    {
        public RegisterRestaurantValidator()
        {
            CascadeRuleFor(x => x.ManagerName)
                .Required();

            CascadeRuleFor(x => x.ManagerEmail)
                .Required()
                .Email();

            CascadeRuleFor(x => x.ManagerPassword)
                .Required()
                .MinLength(8);

            CascadeRuleFor(x => x.RestaurantName)
                .Required();

            CascadeRuleFor(x => x.RestaurantPhoneNumber)
                .Required()
                .PhoneNumber();

            CascadeRuleFor(x => x.AddressLine1)
                .Required();

            CascadeRuleFor(x => x.City)
                .Required();

            CascadeRuleFor(x => x.Postcode)
                .Required()
                .Postcode();
        }
    }
}
