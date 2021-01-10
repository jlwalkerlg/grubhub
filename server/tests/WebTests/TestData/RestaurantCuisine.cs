using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
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
