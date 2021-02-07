namespace Web.Features.Billing.UpdateBillingDetails
{
    public record UpdateBillingDetailsCommand : IRequest
    {
        public string BillingAccountId { get; init; }
        public bool IsBillingEnabled { get; init; }
    }
}
