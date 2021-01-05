using Web.Services.Validation;

namespace Web.Features.Menus.RemoveMenuItem
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
