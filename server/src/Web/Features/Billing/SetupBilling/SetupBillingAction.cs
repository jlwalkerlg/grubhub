using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Billing.SetupBilling
{
    public class SetupBillingAction : Action
    {
        private readonly ISender sender;

        public SetupBillingAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/restaurants/{restaurantId:guid}/billing/setup")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new SetupBillingCommand()
            {
                RestaurantId = restaurantId,
            };

            var result = await sender.Send(query);

            return result ? Ok(result.Value) : Problem(result.Error);
        }
    }
}
