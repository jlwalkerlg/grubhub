using Web.Services.Validation;

namespace Web.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidator : FluentValidator<UpdateMenuItemCommand>
    {
        public UpdateMenuItemValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryName)
                .Required();

            RuleFor(x => x.OldItemName)
                .Required();

            RuleFor(x => x.NewItemName)
                .Required();

            RuleFor(x => x.Description)
                .Required();

            RuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
