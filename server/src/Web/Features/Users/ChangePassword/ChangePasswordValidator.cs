using Web.Services.Validation;

namespace Web.Features.Users.ChangePassword
{
    public class ChangePasswordValidator : RequestValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            CascadeRuleFor(x => x.CurrentPassword)
                .Required();

            CascadeRuleFor(x => x.NewPassword)
                .Required()
                .Password();
        }
    }
}
