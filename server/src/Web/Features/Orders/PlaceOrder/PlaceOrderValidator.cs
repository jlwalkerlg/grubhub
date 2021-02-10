using Web.Services.Validation;

namespace Web.Features.Orders.PlaceOrder
{
    public class PlaceOrderValidator : FluentValidator<PlaceOrderCommand>
    {
        public PlaceOrderValidator()
        {
            CascadeRuleFor(x => x.OrderId)
                .Required();

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
