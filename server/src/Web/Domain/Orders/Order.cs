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
            MobileNumber mobileNumber,
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
            MobileNumber = mobileNumber
                ?? throw new ArgumentNullException(nameof(mobileNumber));
            Address = address;
            PlacedAt = placedAt;
        }

        private Order() { } // EF

        public OrderId Id { get; }
        public UserId UserId { get; }
        public RestaurantId RestaurantId { get; }
        public Money Subtotal { get; }
        public Money DeliveryFee { get; }
        public Money ServiceFee { get; } = Money.FromPounds(0.50m);
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public MobileNumber MobileNumber { get; }
        public Address Address { get; }
        public DateTime PlacedAt { get; }
        public DateTime? ConfirmedAt { get; private set; }
        public DateTime? AcceptedAt { get; private set; }
        public string PaymentIntentId { get; set; }
        public string PaymentIntentClientSecret { get; set; }

        public IReadOnlyList<OrderItem> Items => items;

        public bool AlreadyConfirmed => ConfirmedAt.HasValue;

        public bool AlreadyAccepted => AcceptedAt.HasValue;

        public Money CalculateTotal()
        {
            return Subtotal + DeliveryFee + ServiceFee;
        }

        public void Confirm(DateTime now)
        {
            if (!AlreadyConfirmed)
            {
                ConfirmedAt = now;
                Status = OrderStatus.PaymentConfirmed;
            }
        }

        public void Accept(DateTime now)
        {
            if (!AlreadyAccepted)
            {
                AcceptedAt = now;
                Status = OrderStatus.Accepted;
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
