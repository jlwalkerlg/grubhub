using System;
using Web.Domain.Baskets;

namespace Web.Domain.Orders
{
    public record OrderItem
    {
        internal OrderItem(BasketItem basketItem)
        {
            MenuItemId = basketItem.MenuItemId;
            Quantity = basketItem.Quantity;
        }

        private OrderItem() { } // EF Core

        public Guid MenuItemId { get; }
        public int Quantity { get; }
    }
}
