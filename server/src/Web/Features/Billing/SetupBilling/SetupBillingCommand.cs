using System;

namespace Web.Features.Billing.SetupBilling
{
    public record SetupBillingCommand : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
    }
}
