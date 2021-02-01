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
}
