using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Billing.SetupBilling
{
    public class SetupBillingAction : Action
    {
        private readonly ISender sender;

        public SetupBillingAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPost("/restaurants/{restaurantId:guid}/billing/setup")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new SetupBillingCommand()
            {
                RestaurantId = restaurantId,
            };

            var (link, error) = await sender.Send(query);

            return error ? Problem(error) : Ok(link);
        }
    }
}
