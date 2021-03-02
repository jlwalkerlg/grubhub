using Web.Services.Authentication;

namespace Web.Features.Orders.AcceptOrder
{
    [Authenticate]
    public record AcceptOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
