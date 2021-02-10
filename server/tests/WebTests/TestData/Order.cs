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
        public Guid Id { get; set; } = Guid.NewGuid();

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

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Active;

        [Column("address")]
        public string Address { get; set; }

        [Column("placed_at")]
        public DateTime? PlacedAt { get; set; }

        [Column("payment_intent_id")]
        public string PaymentIntentId { get; set; }

        [Column("confirmed_at")]
        public DateTime? ConfirmedAt { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
