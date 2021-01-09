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
        public string Name { get; set; }

        public List<Restaurant> Restaurants { get; } = new();
    }
}
