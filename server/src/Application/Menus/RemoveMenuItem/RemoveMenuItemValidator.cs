using Application.Validation;

namespace Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidator : FluentValidator<RemoveMenuItemCommand>
    {
        public RemoveMenuItemValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryName)
                .Required();

            RuleFor(x => x.ItemName)
                .Required();
        }
    }
}
