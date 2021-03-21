using Web.Services.Validation;

namespace Web.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryValidator : RequestValidator<RenameMenuCategoryCommand>
    {
        public RenameMenuCategoryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryId)
                .Required();

            RuleFor(x => x.NewName)
                .Required();
        }
    }
}
