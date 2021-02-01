using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.SubmitOrder
{
    [Authenticate]
    public record SubmitOrderCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
    }
}
