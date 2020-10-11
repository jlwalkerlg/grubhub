using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryValidator : FluentValidator<RenameMenuCategoryCommand>
    {
        public RenameMenuCategoryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .Required();

            RuleFor(x => x.OldName)
                .Required();

            RuleFor(x => x.NewName)
                .Required();
        }
    }
}
