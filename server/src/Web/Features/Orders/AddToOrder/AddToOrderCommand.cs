using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.AddToOrder
{
    [Authenticate]
    public record AddToOrderCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
        public int Quantity { get; init; }
    }
}
