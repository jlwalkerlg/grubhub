using Web.Services.Validation;

namespace Web.Features.Orders.PlaceOrder
{
    public class PlaceOrderValidator : RequestValidator<PlaceOrderCommand>
    {
        public PlaceOrderValidator()
        {
            CascadeRuleFor(x => x.Mobile)
                .Required()
                .MobileNumber();

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
