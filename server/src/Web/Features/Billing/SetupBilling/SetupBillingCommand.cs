using System;
using Web.Services.Authentication;

namespace Web.Features.Billing.SetupBilling
{
    [Authenticate]
    public record SetupBillingCommand : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
    }
}
