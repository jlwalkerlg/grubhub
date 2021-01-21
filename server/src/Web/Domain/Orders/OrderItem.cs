using System;

namespace Web.Domain.Orders
{
    public record OrderItem
    {
        public OrderItem(Guid menuItemId)
        {
            if (menuItemId == Guid.Empty)
            {
                throw new ArgumentException("Menu item ID must not be empty.");
            }

            MenuItemId = menuItemId;
        }

        private OrderItem() { } // EF Core

        public Guid MenuItemId { get; }
        public int Quantity { get; private set; } = 1;

        public void IncreaseQuantity()
        {
            Quantity++;
        }

        public void DecreaseQuantity()
        {
            if (Quantity == 1)
            {
                throw new InvalidOperationException("Order item must have at least 1 menu item.");
            }

            Quantity--;
        }
    }
}
