using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("baskets")]
    public record Basket
    {
        private User user;
        private Restaurant restaurant;

        public Basket()
        {
            User = new User();
            Restaurant = new Restaurant();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

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

        public List<BasketItem> Items { get; set; } = new();
    }

    [Table("basket_items")]
    public record BasketItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("basket_id")]
        public int BasketId { get; set; }

        [Column("menu_item_id")]
        public Guid MenuItemId { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
    }
}
