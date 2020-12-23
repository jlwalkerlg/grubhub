using Application.Validation;

namespace Application.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryValidator : FluentValidator<RemoveMenuCategoryCommand>
    {
        public RemoveMenuCategoryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.CategoryName)
                .Required();
        }
    }
}
