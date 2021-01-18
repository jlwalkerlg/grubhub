namespace Web.Features.Menus.UpdateMenuItem
{
    public record UpdateMenuItemRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
