using System;
using System.Collections.Generic;
using Web.Domain.Baskets;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Domain.Orders
{
    public class Order : Entity<Order>
    {
        private List<OrderItem> items = new();

        internal Order(
            OrderId id,
            Basket basket,
            Money subtotal,
            Money deliveryFee,
            Address address,
            DateTime placedAt)
        {
            foreach (var basketItem in basket.Items)
            {
                items.Add(new OrderItem(basketItem));
            }

            Id = id;
            UserId = basket.UserId;
            RestaurantId = basket.RestaurantId;
            Subtotal = subtotal;
            DeliveryFee = deliveryFee with { }; // removes EF warning
            Address = address;
            PlacedAt = placedAt;
        }

        private Order() { } // EF

        public OrderId Id { get; }
        public UserId UserId { get; }
        public RestaurantId RestaurantId { get; }
        public Money Subtotal { get; }
        public Money DeliveryFee { get; }
        public Money ServiceFee { get; } = new Money(0.50m);
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public Address Address { get; private set; }
        public DateTime PlacedAt { get; private set; }
        public string PaymentIntentId { get; set; }
        public string PaymentIntentClientSecret { get; set; }
        public IReadOnlyList<OrderItem> Items => items;

        public Money CalculateTotal()
        {
            return Subtotal + DeliveryFee + ServiceFee;
        }

        public void Confirm()
        {
            if (Status == OrderStatus.Placed)
            {
                Status = OrderStatus.PaymentConfirmed;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected override bool IdentityEquals(Order other)
        {
            return Id == other.Id;
        }
    }
}
