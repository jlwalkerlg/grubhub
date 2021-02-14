using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Subtotal { get; set; } = 1212;

        [Column("delivery_fee")]
        public int DeliveryFee { get; set; } = 151;

        [Column("service_fee")]
        public int ServiceFee { get; set; } = 56;

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        [Column("mobile_number")]
        public string MobileNumber { get; set; } = "07123456789";

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
