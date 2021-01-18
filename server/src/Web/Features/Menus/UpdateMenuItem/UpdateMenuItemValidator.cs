using Web.Services.Validation;

namespace Web.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidator : FluentValidator<UpdateMenuItemCommand>
    {
        public UpdateMenuItemValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryId)
                .Required();

            RuleFor(x => x.ItemId)
                .Required();

            RuleFor(x => x.Name)
                .Required();

            RuleFor(x => x.Description)
                .MaxLength(280);

            RuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
