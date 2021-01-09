using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("menus")]
    public record Menu
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("restaurant_id")]
        public Guid RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public List<MenuCategory> Categories { get; set; } = new();
    }
}
