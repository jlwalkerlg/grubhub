using System.Collections.Generic;
using System;

namespace FoodSnap.Web.Actions.Menus
{
    public class MenuDto
    {
        public Guid RestaurantId { get; set; }
        public List<MenuCategoryDto> Categories { get; set; } = new List<MenuCategoryDto>();
    }

    public class MenuCategoryDto
    {
        public string Name { get; set; }
        public List<MenuItemDto> Items { get; set; } = new List<MenuItemDto>();
    }

    public class MenuItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
