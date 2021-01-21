using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("menu_items")]
    public record MenuItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; } = 9.99m;

        [Column("menu_category_id")]
        public Guid MenuCategoryId { get; set; }
    }
}
