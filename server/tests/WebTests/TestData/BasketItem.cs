using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTests.TestData
{
    [Table("basket_items")]
    public record BasketItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("basket_id")]
        public int BasketId { get; set; }

        [Column("menu_item_id")]
        public Guid MenuItemId { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
    }
}
