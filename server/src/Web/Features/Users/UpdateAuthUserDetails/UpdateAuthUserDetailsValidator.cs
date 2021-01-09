using Web.Services.Validation;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsValidator : FluentValidator<UpdateAuthUserDetailsCommand>
    {
        public UpdateAuthUserDetailsValidator()
        {
            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Email)
                .Required()
                .Email();
        }
    }
}
