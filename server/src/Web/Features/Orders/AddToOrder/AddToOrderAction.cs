using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.AddToOrder
{
    public class AddToOrderAction : Action
    {
        private readonly ISender sender;

        public AddToOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/order")]
        public async Task<IActionResult> Execute([FromBody] AddToOrderCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
