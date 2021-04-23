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

        [Column("number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; private set; }

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

        [Column("address_line1")]
        public string AddressLine1 { get; set; } = Guid.NewGuid().ToString();

        [Column("address_line2")]
        public string AddressLine2 { get; set; }

        [Column("city")]
        public string City { get; set; } = Guid.NewGuid().ToString();

        [Column("postcode")]
        public string Postcode { get; set; } = "MN12 1NM";

        [Column("placed_at")]
        public DateTimeOffset PlacedAt { get; set; } = DateTimeOffset.UtcNow;

        [Column("confirmed_at")]
        public DateTimeOffset? ConfirmedAt { get; set; }

        [Column("accepted_at")]
        public DateTimeOffset? AcceptedAt { get; set; }

        [Column("delivered_at")]
        public DateTimeOffset? DeliveredAt { get; set; }

        [Column("rejected_at")]
        public DateTimeOffset? RejectedAt { get; set; }

        [Column("cancelled_at")]
        public DateTimeOffset? CancelledAt { get; set; }

        [Column("payment_intent_id")]
        public string PaymentIntentId { get; set; } = Guid.NewGuid().ToString();

        [Column("payment_intent_client_secret")]
        public string PaymentIntentClientSecret { get; set; } = Guid.NewGuid().ToString();

        public List<OrderItem> Items { get; set; } = new();
    }

    [Table("order_items")]
    public record OrderItem
    {
        private MenuItem menuItem;

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public string OrderId { get; set; }

        [Column("menu_item_id")]
        public Guid MenuItemId { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem
        {
            get => menuItem;
            set
            {
                menuItem = value;
                Name = menuItem.Name;
                Price = menuItem.Price;
            }
        }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
    }
}
