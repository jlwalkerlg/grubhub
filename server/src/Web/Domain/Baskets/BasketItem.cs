using System;

namespace Web.Domain.Baskets
{
    public record BasketItem
    {
        private int quantity;
        
        internal BasketItem(Guid menuItemId, int quantity)
        {
            if (menuItemId == Guid.Empty)
            {
                throw new ArgumentException("Menu item ID must not be empty.");
            }
            
            MenuItemId = menuItemId;
            Quantity = quantity;
        }

        private BasketItem() { } // EF Core

        public Guid MenuItemId { get; }

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
    }
}
