using Web.Services.Validation;

namespace Web.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemValidator : RequestValidator<RemoveMenuItemCommand>
    {
        public RemoveMenuItemValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryId)
                .Required();

            RuleFor(x => x.ItemId)
                .Required();
        }
    }
}
