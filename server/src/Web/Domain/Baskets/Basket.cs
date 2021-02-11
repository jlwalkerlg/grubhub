using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Domain.Baskets
{
    public class Basket : Entity<Basket>
    {
        private List<BasketItem> items = new();

        public Basket(UserId userId, RestaurantId restaurantId)
        {
            UserId = userId ??
                throw new ArgumentNullException(nameof(userId));
            RestaurantId = restaurantId ??
                throw new ArgumentNullException(nameof(restaurantId));
        }

        private Basket() { } // EF

        public UserId UserId { get; }
        public RestaurantId RestaurantId { get; }
        public IReadOnlyList<BasketItem> Items => items;

        public void AddItem(Guid menuItemId, int quantity)
        {
            var item = items.SingleOrDefault(x => x.MenuItemId == menuItemId);

            if (item == null)
            {
                item = new BasketItem(menuItemId);
                items.Add(item);
            }

            item.Quantity = quantity;
        }

        public Result RemoveItem(Guid menuItemId)
        {
            var orderItem = items.SingleOrDefault(x => x.MenuItemId == menuItemId);

            if (orderItem == null)
            {
                return Error.NotFound("Item not found in basket.");
            }

            if (orderItem.Quantity > 1)
            {
                orderItem.Quantity--;
            }
            else
            {
                items.Remove(orderItem);
            }

            return Result.Ok();
        }

        public Money CalculateSubtotal(Menu menu)
        {
            if (menu.RestaurantId != RestaurantId)
            {
                throw new InvalidOperationException("Wrong menu.");
            }

            var amount = Money.Zero;

            var menuItems = menu.Categories
                .SelectMany(x => x.Items)
                .ToDictionary(x => x.Id);

            foreach (var basketItem in items)
            {
                if (!menuItems.ContainsKey(basketItem.MenuItemId))
                {
                    throw new InvalidOperationException("Menu item not found.");
                }

                var menuItem = menuItems[basketItem.MenuItemId];
                amount += menuItem.Price * basketItem.Quantity;
            }

            return amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, RestaurantId);
        }

        protected override bool IdentityEquals(Basket other)
        {
            return UserId == other.UserId &&
                RestaurantId == other.RestaurantId;
        }
    }
}
