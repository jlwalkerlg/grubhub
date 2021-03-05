
namespace Web.Features.Orders.AcceptOrder
{
    public record AcceptOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
