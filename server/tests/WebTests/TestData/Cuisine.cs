using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("cuisines")]
    public record Cuisine
    {
        [Key]
        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public List<Restaurant> Restaurants { get; } = new();
    }

    [Table("restaurant_cuisines")]
    public record RestaurantCuisine
    {
        [Column("restaurant_id")]
        public Guid RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; }

        [Column("cuisine_name")]
        public string CuisineName { get; set; }

        [ForeignKey(nameof(CuisineName))]
        public Cuisine Cuisine { get; }
    }
}
