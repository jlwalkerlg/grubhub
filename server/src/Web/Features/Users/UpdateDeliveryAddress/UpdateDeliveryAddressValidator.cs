using Web.Services.Validation;

namespace Web.Features.Users.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressValidator : FluentValidator<UpdateDeliveryAddressCommand>
    {
        public UpdateDeliveryAddressValidator()
        {
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
