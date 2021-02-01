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

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Quantity must not be less than 1.");
                }

                quantity = value;
            }
        }

        public void DecreaseQuantity()
        {
            if (quantity == 1)
            {
                throw new InvalidOperationException("Order item must have at least 1 menu item.");
            }

            quantity--;
        }
    }
}
