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
        public string Description { get; set; } = Guid.NewGuid().ToString();

        [Column("price")]
        public int Price { get; set; } = 999;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("menu_category_id")]
        public Guid MenuCategoryId { get; set; }
    }
}
