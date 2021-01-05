namespace Web.Features.Menus.RenameMenuCategory
{
    public record RenameMenuCategoryRequest
    {
        public string NewName { get; init; }
    }
}
