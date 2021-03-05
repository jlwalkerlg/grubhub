
namespace Web.Features.Orders.RejectOrder
{
    public record RejectOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
