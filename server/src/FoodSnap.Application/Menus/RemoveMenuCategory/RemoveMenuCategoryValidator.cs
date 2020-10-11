using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.RemoveMenuCategory
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
