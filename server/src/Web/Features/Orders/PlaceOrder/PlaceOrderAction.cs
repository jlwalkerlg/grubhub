using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Orders.PlaceOrder
{
    public class PlaceOrderAction : Action
    {
        private readonly ISender sender;

        public PlaceOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/orders/{orderId:guid}/place")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid orderId,
            [FromBody] PlaceOrderRequest request)
        {
            var command = new PlaceOrderCommand()
            {
                OrderId = orderId,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                City = request.City,
                Postcode = request.Postcode,
            };

            var result = await sender.Send(command);

            return result ? Ok(result.Value) : Error(result.Error);
        }
    }
}
