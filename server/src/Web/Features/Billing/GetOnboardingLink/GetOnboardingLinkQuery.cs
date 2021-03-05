using System;

namespace Web.Features.Billing.GetOnboardingLink
{
    public record GetOnboardingLinkQuery : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
    }
}
