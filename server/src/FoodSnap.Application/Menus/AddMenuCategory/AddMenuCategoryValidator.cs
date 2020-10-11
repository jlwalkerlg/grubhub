using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.AddMenuCategory
{
    public class AddMenuCategoryValidator : FluentValidator<AddMenuCategoryCommand>
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
