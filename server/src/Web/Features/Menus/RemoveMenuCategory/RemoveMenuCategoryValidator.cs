using Web.Services.Validation;

namespace Web.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryValidator : RequestValidator<RemoveMenuCategoryCommand>
    {
        public RemoveMenuCategoryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryId)
                .Required();
        }
    }
}
