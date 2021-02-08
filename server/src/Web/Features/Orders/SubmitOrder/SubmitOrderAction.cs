using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.SubmitOrder
{
    public class SubmitOrderAction : Action
    {
        private readonly ISender sender;

        public SubmitOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/order/{restaurantId:guid}/submit")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var command = new SubmitOrderCommand()
            {
                RestaurantId = restaurantId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
