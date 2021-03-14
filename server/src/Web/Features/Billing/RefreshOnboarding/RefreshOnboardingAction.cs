using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Features.Billing.SetupBilling;
using Web.Services.Authentication;

namespace Web.Features.Billing.RefreshOnboarding
{
    public class RefreshOnboardingAction : Action
    {
        private readonly IAuthenticator authenticator;
        private readonly AppSettings settings;
        private readonly ISender sender;

        public RefreshOnboardingAction(
            IAuthenticator authenticator, AppSettings settings, ISender sender)
        {
            this.authenticator = authenticator;
            this.settings = settings;
            this.sender = sender;
        }

        [HttpGet("/stripe/onboarding/refresh")]
        public async Task<IActionResult> Execute(
            [FromQuery(Name = "restaurant_id")] Guid restaurantId)
        {
            if (!authenticator.IsAuthenticated)
            {
                return Redirect($"{settings.ClientUrl}/login?redirect_to={settings.StripeOnboardingRefreshUrl}?restaurant_id={restaurantId}");
            }

            var command = new SetupBillingCommand()
            {
                RestaurantId = restaurantId,
            };

            var (link, error) = await sender.Send(command);

            return error ? Problem(error) : Redirect(link);
        }
    }
}
