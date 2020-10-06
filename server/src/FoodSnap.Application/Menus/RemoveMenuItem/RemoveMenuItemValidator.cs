using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidator : FluentValidator<RemoveMenuItemCommand>
    {
        public RemoveMenuItemValidator()
        {
            RuleFor(x => x.MenuId)
                .Required();

            RuleFor(x => x.Category)
                .Required();

            RuleFor(x => x.Item)
                .Required();
        }
    }
}
