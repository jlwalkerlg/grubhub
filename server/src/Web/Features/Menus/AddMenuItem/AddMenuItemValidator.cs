using Web.Services.Validation;

namespace Web.Features.Menus.AddMenuItem
{
    public class AddMenuItemValidator : RequestValidator<AddMenuItemCommand>
    {
        public AddMenuItemValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();

            CascadeRuleFor(x => x.CategoryId)
                .Required();

            CascadeRuleFor(x => x.Name)
                .Required();

            CascadeRuleFor(x => x.Description)
                .MaxLength(280);

            CascadeRuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
