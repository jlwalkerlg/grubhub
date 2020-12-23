using Application.Users.Login;
using Application.Validation;

namespace Application.Restaurants.Login
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
