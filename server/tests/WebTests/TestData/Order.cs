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

        [Column("number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; private set; }

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

        [Column("delivery_fee")]
        public decimal DeliveryFee { get; set; } = 1.51m;

        [Column("service_fee")]
        public decimal ServiceFee { get; set; } = 0.56m;

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        [Column("mobile_number")]
        public string MobileNumber { get; set; } = "07123456789";

        [Column("address")]
        public string Address { get; set; } = Guid.NewGuid().ToString();

        [Column("placed_at")]
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        [Column("confirmed_at")]
        public DateTime? ConfirmedAt { get; set; }

        [Column("accepted_at")]
        public DateTime? AcceptedAt { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [Column("payment_intent_id")]
        public string PaymentIntentId { get; set; } = Guid.NewGuid().ToString();

        [Column("payment_intent_client_secret")]
        public string PaymentIntentClientSecret { get; set; } = Guid.NewGuid().ToString();

        public List<OrderItem> Items { get; set; } = new();
    }
}
