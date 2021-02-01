using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Domain.Orders
{
    public class Order : Entity<Order>
    {
        private List<OrderItem> items = new();

        public Order(OrderId id, UserId userId, RestaurantId restaurantId)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (restaurantId == null)
            {
                throw new ArgumentNullException(nameof(restaurantId));
            }

            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
        }

        private Order() { } // EF Core

        public OrderId Id { get; }
        public UserId UserId { get; }
        public RestaurantId RestaurantId { get; }
        public OrderStatus Status { get; private set; } = OrderStatus.Active;
        public IReadOnlyList<OrderItem> Items => items;

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        public void AddItem(Guid menuItemId, int quantity)
        {
            var item = items.SingleOrDefault(x => x.MenuItemId == menuItemId);

            if (item == null)
            {
                item = new OrderItem(menuItemId);
                items.Add(item);
            }

            item.Quantity = quantity;
        }

        public Result RemoveItem(Guid menuItemId)
        {
            var orderItem = items.SingleOrDefault(x => x.MenuItemId == menuItemId);

            if (orderItem == null)
            {
                return Error.NotFound("Item not found on order.");
            }

            if (orderItem.Quantity > 1)
            {
                orderItem.DecreaseQuantity();
            }
            else
            {
                items.Remove(orderItem);
            }

            return Result.Ok();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected override bool IdentityEquals(Order other)
        {
            return Id == other.Id;
        }
    }
}
