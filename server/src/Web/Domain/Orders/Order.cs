using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Baskets;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;

namespace Web.Domain.Orders
{
    public class Order : Entity<Order>
    {
        private List<OrderItem> items = new();

        internal Order(
            OrderId id,
            Basket basket,
            Menu menu,
            Money deliveryFee,
            MobileNumber mobileNumber,
            Address address,
            DateTimeOffset placedAt)
        {
            Id = id;
            UserId = basket.UserId;
            RestaurantId = basket.RestaurantId;
            DeliveryFee = deliveryFee;
            MobileNumber = mobileNumber ?? throw new ArgumentNullException(nameof(mobileNumber));
            Address = address;
            PlacedAt = placedAt;

            foreach (var basketItem in basket.Items)
            {
                items.Add(new OrderItem(menu.GetItem(basketItem.MenuItemId), basketItem.Quantity));
            }
        }

        private Order() { } // EF

        public OrderId Id { get; }
        public UserId UserId { get; }
        public RestaurantId RestaurantId { get; }
        public Money DeliveryFee { get; }
        public Money ServiceFee { get; } = Money.FromPounds(0.50m);
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public MobileNumber MobileNumber { get; }
        public Address Address { get; }
        public DateTimeOffset PlacedAt { get; }
        public DateTimeOffset? ConfirmedAt { get; private set; }
        public DateTimeOffset? AcceptedAt { get; private set; }
        public DateTimeOffset? DeliveredAt { get; private set; }
        public DateTimeOffset? RejectedAt { get; private set; }
        public DateTimeOffset? CancelledAt { get; private set; }
        public string PaymentIntentId { get; set; }
        public string PaymentIntentClientSecret { get; set; }

        public IReadOnlyList<OrderItem> Items => items;
        public Money Subtotal => items.Aggregate(Money.Zero, (acc, item) => acc + item.Price * item.Quantity);

        public bool Confirmed => ConfirmedAt.HasValue;

        public bool Accepted => AcceptedAt.HasValue;
        public bool Delivered => DeliveredAt.HasValue;
        public bool Rejected => RejectedAt.HasValue;
        public bool Cancelled => CancelledAt.HasValue;

        public Money CalculateTotal()
        {
            return Subtotal + DeliveryFee + ServiceFee;
        }

        public void Confirm(DateTimeOffset now)
        {
            if (!Confirmed)
            {
                ConfirmedAt = now;
                Status = OrderStatus.PaymentConfirmed;
            }
        }

        public Result Accept(DateTimeOffset now)
        {
            if (!Confirmed)
            {
                return Error.BadRequest("Only orders for which payment has been confirmed can be accepted.");
            }

            if (!Accepted)
            {
                AcceptedAt = now;
                Status = OrderStatus.Accepted;
            }

            return Result.Ok();
        }

        public Result Deliver(DateTimeOffset now)
        {
            if (!Accepted)
            {
                return Error.BadRequest("Order must be accepted before it can be delivered.");
            }

            if (!Delivered)
            {
                DeliveredAt = now;
                Status = OrderStatus.Delivered;
            }

            return Result.Ok();
        }

        public Result Reject(DateTimeOffset time)
        {
            if (!Confirmed) return Error.BadRequest("Order must be confirmed before it can be rejected.");

            if (Accepted) return Error.BadRequest("Order was already accepted.");

            if (!Rejected)
            {
                Status = OrderStatus.Rejected;
                RejectedAt = time;
            }

            return Result.Ok();
        }


        public Result Cancel(DateTimeOffset time)
        {
            if (!Accepted) return Error.BadRequest("Only accepted orders can be cancelled.");

            if (Delivered) return Error.BadRequest("Order was already delivered.");

            if (!Cancelled)
            {
                Status = OrderStatus.Cancelled;
                CancelledAt = time;
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
