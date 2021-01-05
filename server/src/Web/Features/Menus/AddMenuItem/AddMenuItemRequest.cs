namespace Web.Features.Menus.AddMenuItem
{
    public record AddMenuItemRequest
    {
        public string CategoryName { get; init; }
        public string ItemName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
