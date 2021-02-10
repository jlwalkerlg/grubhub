using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.ConfirmOrder
{
    [Authenticate]
    public record ConfirmOrderCommand : IRequest
    {
        public Guid OrderId { get; init; }
    }
}
