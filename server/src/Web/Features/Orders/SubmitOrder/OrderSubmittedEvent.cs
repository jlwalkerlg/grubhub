using System;
using Web.Domain.Orders;
using Web.Features.Events;

namespace Web.Features.Orders.SubmitOrder
{
    public record OrderSubmittedEvent(OrderId OrderId, DateTime CreatedAt) : Event(CreatedAt);
}
