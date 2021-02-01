using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.RemoveFromOrder
{
    [Authenticate]
    public record RemoveFromOrderCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public Guid MenuItemId { get; init; }
    }
}
