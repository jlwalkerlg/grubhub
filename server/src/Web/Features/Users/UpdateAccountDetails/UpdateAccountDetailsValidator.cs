using Web.Services.Validation;

namespace Web.Features.Users.UpdateAccountDetails
{
    public class UpdateAccountDetailsValidator : FluentValidator<UpdateAccountDetailsCommand>
    {
        public UpdateAccountDetailsValidator()
        {
            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.MobileNumber)
                .Required()
                .MobileNumber();
        }
    }
}
