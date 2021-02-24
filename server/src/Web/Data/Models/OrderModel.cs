using System;
using Web.Features.Orders.GetActiveOrder;

namespace Web.Data.Models
{
    public record OrderModel
    {
        public string id { get; init; }
        public int number { get; init; }
        public Guid user_id { get; init; }
        public Guid restaurant_id { get; init; }
        public decimal subtotal { get; init; }
        public decimal delivery_fee { get; init; }
        public decimal service_fee { get; init; }
        public string status { get; init; }
        public string address { get; init; }
        public DateTime placed_at { get; init; }
        public string restaurant_name { get; init; }
        public string restaurant_address { get; init; }
        public string restaurant_phone_number { get; init; }
        public string payment_intent_client_secret { get; init; }

        public OrderDto ToDto()
        {
            return new OrderDto()
            {
                Id = id,
                Number = number,
                UserId = user_id,
                RestaurantId = restaurant_id,
                Subtotal = subtotal,
                DeliveryFee = delivery_fee,
                ServiceFee = service_fee,
                Status = status,
                Address = address,
                PlacedAt = placed_at,
                PaymentIntentClientSecret = payment_intent_client_secret,
                RestaurantName = restaurant_name,
                RestaurantAddress = restaurant_address,
                RestaurantPhoneNumber = restaurant_phone_number,
            };
        }
    }

    public record OrderItemModel
    {
        public int id { get; init; }
        public string order_id { get; init; }
        public Guid menu_item_id { get; init; }
        public string menu_item_name { get; init; }
        public string menu_item_description { get; init; }
        public decimal menu_item_price { get; init; }
        public int quantity { get; init; }

        public OrderItemDto ToDto()
        {
            return new OrderItemDto()
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
