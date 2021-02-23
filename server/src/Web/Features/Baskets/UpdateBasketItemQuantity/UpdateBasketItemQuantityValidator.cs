using Web.Services.Validation;

namespace Web.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityValidator : FluentValidator<UpdateBasketItemQuantityCommand>
    {
        public UpdateBasketItemQuantityValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();

            CascadeRuleFor(x => x.MenuItemId)
                .Required();

            CascadeRuleFor(x => x.Quantity)
                .Min(1);
        }
    }
}