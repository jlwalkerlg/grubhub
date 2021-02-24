using System;
using Web.Features.Baskets;

namespace Web.Data.Models
{
    public record BasketEntry
    {
        public int id { get; init; }
        public Guid user_id { get; init; }
        public Guid restaurant_id { get; init; }

        public BasketDto ToDto()
        {
            return new BasketDto()
            {
                UserId = user_id,
                RestaurantId = restaurant_id,
            };
        }
    }

    public record BasketItemEntry
    {
        public int id { get; init; }
        public Guid menu_item_id { get; init; }
        public string menu_item_name { get; init; }
        public string menu_item_description { get; init; }
        public decimal menu_item_price { get; init; }
        public int quantity { get; init; }

        public BasketItemDto ToDto()
        {
            return new BasketItemDto()
            {
                MenuItemId = menu_item_id,
                MenuItemName = menu_item_name,
                MenuItemDescription = menu_item_description,
                MenuItemPrice = menu_item_price,
                Quantity = quantity,
            };
        }
    }
}
