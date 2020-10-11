using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    public class AddMenuItemValidator : FluentValidator<AddMenuItemCommand>
    {
        public AddMenuItemValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();

            CascadeRuleFor(x => x.CategoryName)
                .Required();

            CascadeRuleFor(x => x.ItemName)
                .Required();

            CascadeRuleFor(x => x.Description)
                .Required();

            CascadeRuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
