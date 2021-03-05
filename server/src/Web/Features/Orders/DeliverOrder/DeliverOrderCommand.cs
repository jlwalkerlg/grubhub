using System;

namespace Web.Features.Orders.DeliverOrder
{
    public record DeliverOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
