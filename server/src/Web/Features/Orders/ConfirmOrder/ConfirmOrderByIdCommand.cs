namespace Web.Features.Orders.ConfirmOrder
{
    public record ConfirmOrderByIdCommand : IRequest
    {
        public string Id { get; init; }
    }
}
