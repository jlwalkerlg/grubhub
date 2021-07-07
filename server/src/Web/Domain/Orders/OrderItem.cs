using Web.Domain.Menus;

namespace Web.Domain.Orders
{
    public record OrderItem
    {
        internal OrderItem(MenuItem menuItem, int quantity)
        {
            Name = menuItem.Name;
            Price = menuItem.Price;
            Quantity = quantity;
        }

        private OrderItem() { } // EF Core

        public string Name { get; }
        public Money Price { get; }
        public int Quantity { get; }
    }
}
