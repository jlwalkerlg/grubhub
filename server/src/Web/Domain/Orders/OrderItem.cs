using System;
using Web.Domain.Menus;

namespace Web.Domain.Orders
{
    public record OrderItem
    {
        internal OrderItem(MenuItem menuItem, int quantity)
        {
            MenuItemId = menuItem.Id;
            Name = menuItem.Name;
            Price = menuItem.Price with {}; // removes EF warning
            Quantity = quantity;
        }

        private OrderItem() { } // EF Core

        public Guid MenuItemId { get; }
        public string Name { get; }
        public Money Price { get; }
        public int Quantity { get; }
    }
}
