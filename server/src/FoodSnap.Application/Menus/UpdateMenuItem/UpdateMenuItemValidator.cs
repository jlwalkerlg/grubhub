using FoodSnap.Application.Validation;

namespace FoodSnap.Application.Menus.UpdateMenuItem
{
    public class UpdateMenuItemValidator : FluentValidator<UpdateMenuItemCommand>
    {
        public UpdateMenuItemValidator()
        {
            RuleFor(x => x.MenuId)
                .Required();

            RuleFor(x => x.Category)
                .Required();

            RuleFor(x => x.Item)
                .Required();

            RuleFor(x => x.Name)
                .Required();

            RuleFor(x => x.Description)
                .Required();

            RuleFor(x => x.Price)
                .Required()
                .Min(0);
        }
    }
}
