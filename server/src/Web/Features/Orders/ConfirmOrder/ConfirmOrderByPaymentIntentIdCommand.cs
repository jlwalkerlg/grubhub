namespace Web.Features.Orders.ConfirmOrder
{
    public record ConfirmOrderByPaymentIntentIdCommand : IRequest
    {
        public string PaymentIntentId { get; init; }
    }
}
