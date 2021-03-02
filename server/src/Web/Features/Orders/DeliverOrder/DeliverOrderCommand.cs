using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.DeliverOrder
{
    [Authenticate]
    public record DeliverOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
