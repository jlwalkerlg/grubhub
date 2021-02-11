using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.ConfirmOrder
{
    public record ConfirmOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
