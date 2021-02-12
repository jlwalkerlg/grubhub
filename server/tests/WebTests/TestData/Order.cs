using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Domain;
using Web.Domain.Orders;

namespace WebTests.TestData
{
    [Table("orders")]
    public record Order
    {
        private User user;
        private Restaurant restaurant;

        public Order()
        {
            User = new User();
            Restaurant = new Restaurant();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("user_id")]
        public Guid UserId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public User User
        {
            get => user;
            set
            {
                user = value;
                UserId = value == null ? Guid.Empty : value.Id;
            }
        }

        [Column("restaurant_id")]
        public Guid RestaurantId { get; private set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant
        {
            get => restaurant;
            set
            {
                restaurant = value;
                RestaurantId = value == null ? Guid.Empty : value.Id;
            }
        }

        [Column("subtotal")]
        public decimal Subtotal { get; set; } = 12.12m;

        [Column("delivery_fee")]
        public decimal DeliveryFee { get; set; } = 1.51m;

        [Column("service_fee")]
        public decimal ServiceFee { get; set; } = 0.56m;

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        [Column("address")]
        public string Address { get; set; } = Guid.NewGuid().ToString();

        [Column("placed_at")]
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        [Column("payment_intent_id")]
        public string PaymentIntentId { get; set; } = Guid.NewGuid().ToString();

        [Column("payment_intent_client_secret")]
        public string PaymentIntentClientSecret { get; set; } = Guid.NewGuid().ToString();

        public List<OrderItem> Items { get; set; } = new();
    }
}
