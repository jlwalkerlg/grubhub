using System;
using System.Collections.Generic;

namespace Web.Features.Orders.GetActiveOrder
{
    public record OrderDto
    {
        public string Id { get; init; }
        public int Number { get; init; }
        public Guid UserId { get; init; }
        public Guid RestaurantId { get; init; }
        public decimal Subtotal { get; init; }
        public decimal DeliveryFee { get; init; }
        public decimal ServiceFee { get; init; }
        public string Status { get; init; }
        public string Address { get; init; }
        public DateTime PlacedAt { get; init; }
        public DateTime? ConfirmedAt { get; init; }
        public string RestaurantName { get; init; }
        public string RestaurantAddress { get; init; }
        public string RestaurantPhoneNumber { get; init; }
        public string PaymentIntentClientSecret { get; init; }
        public List<OrderItemDto> Items { get; init; } = new();
    }

    public record OrderItemDto
    {
        public Guid MenuItemId { get; init; }
        public string MenuItemName { get; init; }
        public string MenuItemDescription { get; init; }
        public decimal MenuItemPrice { get; init; }
        public int Quantity { get; init; }
    }
}
