namespace Web.Features.Orders.ConfirmOrder
{
    public record ConfirmOrderCommand : IRequest
    {
        public string PaymentIntentId { get; init; }
    }
}
