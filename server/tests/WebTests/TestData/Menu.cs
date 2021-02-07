using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("menus")]
    public record Menu
    {
        private Restaurant restaurant;

        public Menu()
        {
            Restaurant = new Restaurant();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("restaurant_id")]
        public Guid RestaurantId { get; private set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant
        {
            get => restaurant;
            set
            {
                restaurant = value;
                RestaurantId = value == null ? value.Id : Guid.Empty;
            }
        }

        public List<MenuCategory> Categories { get; set; } = new();
    }
}
