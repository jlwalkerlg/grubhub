using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Features.Billing.GenerateOnboardingLink;
using Web.Services.Authentication;

namespace Web.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly Config config;
        private readonly ISender sender;

        public RefreshOnboardingAction(
            IAuthenticator authenticator, Config config, ISender sender)
        {
            this.authenticator = authenticator;
            this.config = config;
            this.sender = sender;
        }

        [HttpGet("/stripe/onboarding/refresh")]
        public async Task<IActionResult> Execute(
            [FromQuery(Name = "restaurant_id")] Guid restaurantId)
        {
            if (!authenticator.IsAuthenticated)
            {
                return Redirect($"{config.ClientUrl}/login?redirect_to={config.StripeOnboardingRefreshUrl}?restaurant_id={restaurantId}");
            }

            var query = new GenerateOnboardingLinkQuery()
            {
                RestaurantId = restaurantId,
            };

            var result = await sender.Send(query);

            return result ? Redirect(result.Value) : Error(result.Error);
        }
    }
}
