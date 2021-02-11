using Web.Services.Validation;

namespace Web.Features.Baskets.AddToBasket
{
    public class AddToBasketValidator : FluentValidator<AddToBasketCommand>
    {
        public AddToBasketValidator()
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
