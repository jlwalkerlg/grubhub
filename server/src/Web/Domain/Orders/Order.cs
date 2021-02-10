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

        public Address Address { get; private set; }

        public DateTime? PlacedAt { get; private set; }

        public IReadOnlyList<OrderItem> Items => items;

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        internal Result Place(Address address, DateTime time)
        {
            if (Status != OrderStatus.Active && Status != OrderStatus.Placed)
            {
                return Error.BadRequest("Order not active.");
            }

            if (!items.Any())
            {
                return Error.BadRequest("Order is empty.");
            }

            Status = OrderStatus.Placed;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            PlacedAt = time;

            return Result.Ok();
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

            Status = OrderStatus.Active;
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

            Status = OrderStatus.Active;

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
