using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("menu_categories")]
    public record MenuCategory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Column("menu_id")]
        public int MenuId { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        public List<MenuItem> Items { get; set; } = new();
    }
}
