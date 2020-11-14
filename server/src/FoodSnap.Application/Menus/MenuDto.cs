using System.Collections.Generic;
using System;

namespace FoodSnap.Application.Menus
{
    public record MenuDto
    {
        public Guid RestaurantId { get; init; }
        public List<MenuCategoryDto> Categories { get; init; } = new();
    }

    public record MenuCategoryDto
    {
        public string Name { get; init; }
        public List<MenuItemDto> Items { get; init; } = new();
    }

    public record MenuItemDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
