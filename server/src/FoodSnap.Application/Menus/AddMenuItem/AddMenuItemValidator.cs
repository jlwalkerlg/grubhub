using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    public class AddMenuItemValidator : FluentValidator<AddMenuItemCommand>
    {
        public AddMenuItemValidator()
        {
            CascadeRuleFor(x => x.MenuId)
                .Required();

            CascadeRuleFor(x => x.Category)
                .Required();

            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Description)
                .Required();

            CascadeRuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
