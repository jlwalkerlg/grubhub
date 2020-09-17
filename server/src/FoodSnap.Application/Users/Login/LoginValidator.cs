using FoodSnap.Application.Users.Login;
using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Restaurants.Login
{
    public class LoginValidator : FluentValidator<LoginCommand>
    {
        public LoginValidator()
        {
            CascadeRuleFor(x => x.Email)
                .Required()
                .Email();

            CascadeRuleFor(x => x.Password)
                .Required();
        }
    }
}
