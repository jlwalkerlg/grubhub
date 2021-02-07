using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Billing.GenerateOnboardingLink
{
    public class GenerateOnboardingLinkAction : Action
    {
        private readonly ISender sender;

        public GenerateOnboardingLinkAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/restaurants/{restaurantId:guid}/billing/onboarding/link")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new GenerateOnboardingLinkQuery()
            {
                RestaurantId = restaurantId,
            };

            var result = await sender.Send(query);

            return result ? Ok(result.Value) : Error(result.Error);
        }
    }
}
