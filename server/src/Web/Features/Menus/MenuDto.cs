using System;
using System.Collections.Generic;

namespace Web.Features.Menus
{
    public record MenuDto
    {
        public Guid RestaurantId { get; init; }
        public List<MenuCategoryDto> Categories { get; init; } = new();
    }

    public record MenuCategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<MenuItemDto> Items { get; init; } = new();
    }

    public record MenuItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
