using System;

namespace Web.Features.Billing.GetBillingDetails
{
    public record BillingDetails
    {
        public string Id { get; init; }
        public Guid RestaurantId { get; init; }
        public bool IsBillingEnabled { get; init; }
    }
}
