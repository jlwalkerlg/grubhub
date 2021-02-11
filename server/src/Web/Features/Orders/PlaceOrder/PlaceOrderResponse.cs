namespace Web.Features.Orders.PlaceOrder
{
    public record PlaceOrderResponse
    {
        public string OrderId { get; init; }
        public string PaymentIntentClientSecret { get; init; }
    }
}
