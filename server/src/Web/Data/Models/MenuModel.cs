using System;
using Web.Features.Menus;

namespace Web.Data.Models
{
    public record MenuModel
    {
        public int id { get; init; }
        public Guid restaurant_id { get; init; }

        public MenuDto ToDto()
        {
            return new MenuDto()
            {
                RestaurantId = restaurant_id,
            };
        }
    }

    public record MenuCategoryModel
    {
        public Guid id { get; init; }
        public int menu_id { get; init; }
        public string name { get; init; }

        public MenuCategoryDto ToDto()
        {
            return new MenuCategoryDto()
            {
                Id = id,
                Name = name,
            };
        }
    }

    public record MenuItemModel
    {
        public Guid id { get; init; }
        public Guid menu_category_id { get; init; }
        public string name { get; init; }
        public string description { get; init; }
        public decimal price { get; init; }

        public MenuItemDto ToDto()
        {
            return new MenuItemDto()
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
            };
        }
    }
}
