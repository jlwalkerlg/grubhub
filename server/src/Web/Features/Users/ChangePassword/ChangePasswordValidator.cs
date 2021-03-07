using Web.Services.Validation;

namespace Web.Features.Users.ChangePassword
{
    public class ChangePasswordValidator : FluentValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            CascadeRuleFor(x => x.Password)
                .Required()
                .Password();
        }
    }
}
