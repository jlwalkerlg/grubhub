namespace Web.Features.Billing
{
    public record PaymentIntent
    {
        public string Id { get; init; }
        public string ClientSecret { get; init; }
    }
}
