using Web.Services.Validation;

namespace Web.Features.Users.Register
{
    public class RegisterValidator : FluentValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Email)
                .Required()
                .Email();

            CascadeRuleFor(x => x.Password)
                .Required()
                .Password();
        }
    }
}
