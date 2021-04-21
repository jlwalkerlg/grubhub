using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderByIdAction : Action
    {
        private readonly ISender sender;

        public ConfirmOrderByIdAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/orders/{orderId:guid}/confirm")]
        public async Task<IActionResult> Execute([FromRoute] string orderId)
        {
            var command = new ConfirmOrderByIdCommand()
            {
                Id = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
