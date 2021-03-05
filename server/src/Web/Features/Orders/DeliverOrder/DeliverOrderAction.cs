using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.DeliverOrder
{
    public class DeliverOrderAction : Action
    {
        private readonly ISender sender;

        public DeliverOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/orders/{orderId}/deliver")]
        public async Task<IActionResult> Execute([FromRoute] string orderId)
        {
            var command = new DeliverOrderCommand()
            {
                OrderId = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
