using System;
using Web.Services.Authentication;

namespace Web.Features.Billing.GetOnboardingLink
{
    [Authenticate]
    public record GetOnboardingLinkQuery : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
    }
}
