using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.ConfirmOrder
{
    public class ConfirmOrderAction : Action
    {
        private readonly ISender sender;

        public ConfirmOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/orders/{id:guid}/confirm")]
        public async Task<IActionResult> Execute([FromRoute] Guid id)
        {
            var command = new ConfirmOrderCommand()
            {
                OrderId = id,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
