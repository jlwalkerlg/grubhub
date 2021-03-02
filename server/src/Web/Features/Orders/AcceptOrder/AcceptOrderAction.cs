using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.AcceptOrder
{
    public class AcceptOrderAction : Action
    {
        private readonly ISender sender;

        public AcceptOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/orders/{orderId:guid}/accept")]
        public async Task<IActionResult> AcceptOrder([FromRoute] string orderId)
        {
            var command = new AcceptOrderCommand()
            {
                OrderId = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
