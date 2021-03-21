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

        public List<MenuCategory> Categories { get; set; } = new();
    }

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
        public decimal Price { get; set; } = 9.99m;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("menu_category_id")]
        public Guid MenuCategoryId { get; set; }
    }
}
