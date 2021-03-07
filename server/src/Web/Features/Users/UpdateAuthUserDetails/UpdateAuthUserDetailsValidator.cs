using Web.Services.Validation;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsValidator : FluentValidator<UpdateAuthUserDetailsCommand>
    {
        public UpdateAuthUserDetailsValidator()
        {
            CascadeRuleFor(x => x.FirstName)
                .Required();

            CascadeRuleFor(x => x.LastName)
                .Required();

            CascadeRuleFor(x => x.Email)
                .Required()
                .Email();
        }
    }
}
