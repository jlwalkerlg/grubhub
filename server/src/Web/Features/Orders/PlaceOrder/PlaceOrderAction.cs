using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("/restaurants/{restaurantId:guid}/orders")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] PlaceOrderRequest request)
        {
            var command = new PlaceOrderCommand()
            {
                RestaurantId = restaurantId,
                Mobile = request.Mobile,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                Postcode = request.Postcode,
            };

            var (orderId, error) = await sender.Send(command);

            return error ? Problem(error) : Ok(orderId);
        }
    }
}
