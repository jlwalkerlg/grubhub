using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Billing.GetOnboardingLink
{
    public class GetOnboardingLinkAction : Action
    {
        private readonly ISender sender;

        public GetOnboardingLinkAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpGet("/restaurants/{restaurantId:guid}/billing/onboarding/link")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new GetOnboardingLinkQuery()
            {
                RestaurantId = restaurantId,
            };

            var (link, error) = await sender.Send(query);

            return error ? Problem(error) : Ok(link);
        }
    }
}
