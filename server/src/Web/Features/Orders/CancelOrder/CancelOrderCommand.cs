namespace Web.Features.Orders.CancelOrder
{
    public record CancelOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }
}
