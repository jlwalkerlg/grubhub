using System;
using Web.Services.Authentication;

namespace Web.Features.Billing.GenerateOnboardingLink
{
    [Authenticate]
    public record GenerateOnboardingLinkQuery : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
    }
}
