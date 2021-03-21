using Web.Services.Validation;

namespace Web.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryValidator : RequestValidator<AddMenuCategoryCommand>
    {
        public AddMenuCategoryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.Name)
                .Required();
        }
    }
}
