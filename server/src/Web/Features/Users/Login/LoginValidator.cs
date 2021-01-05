using Web.Features.Users.Login;
using Web.Services.Validation;

namespace Web.Features.Restaurants.Login
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
