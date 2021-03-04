using Web.Services.Authentication;

namespace Web.Features.Orders.RejectOrder
{
    [Authenticate]
    public record RejectOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
