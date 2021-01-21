using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.RemoveFromOrder
{
    public class RemoveFromOrderAction : Action
    {
        private readonly ISender sender;

        public RemoveFromOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpDelete("/order/items/{menuItemId}")]
        public async Task<IActionResult> Execute([FromRoute] Guid menuItemId)
        {
            var command = new RemoveFromOrderCommand()
            {
                MenuItemId = menuItemId,
            };

            var result = await sender.Send(command);

            return result ? StatusCode(204) : Error(result.Error);
        }
    }
}
