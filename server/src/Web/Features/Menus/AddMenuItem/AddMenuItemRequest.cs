using System;

namespace Web.Features.Menus.AddMenuItem
{
    public record AddMenuItemRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
