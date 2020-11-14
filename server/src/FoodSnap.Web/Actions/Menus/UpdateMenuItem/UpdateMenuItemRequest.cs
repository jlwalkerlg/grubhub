namespace FoodSnap.Web.Actions.Menus.UpdateMenuItem
{
    public record UpdateMenuItemRequest
    {
        public string NewItemName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
