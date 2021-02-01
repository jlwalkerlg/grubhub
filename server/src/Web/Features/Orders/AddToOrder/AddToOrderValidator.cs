using Web.Services.Validation;

namespace Web.Features.Orders.AddToOrder
{
    public class AddToOrderValidator : FluentValidator<AddToOrderCommand>
    {
        public AddToOrderValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.MenuItemId)
                .Required();

            RuleFor(x => x.Quantity)
                .Min(1);
        }
    }
}
