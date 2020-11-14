namespace FoodSnap.Web.Actions.Menus.RenameMenuCategory
{
    public record RenameMenuCategoryRequest
    {
        public string NewName { get; init; }
    }
}
