using System;

namespace Web.Domain.Baskets
{
    public record BasketItem
    {
        public BasketItem(Guid menuItemId)
        {
            if (menuItemId == Guid.Empty)
            {
                throw new ArgumentException("Menu item ID must not be empty.");
            }

            MenuItemId = menuItemId;
        }

        private BasketItem() { } // EF Core

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
    }
}
