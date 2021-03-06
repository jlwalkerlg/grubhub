namespace Web.Features.Orders.PlaceOrder
{
    public record PlaceOrderRequest
    {
        public string Mobile { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
    }
}
