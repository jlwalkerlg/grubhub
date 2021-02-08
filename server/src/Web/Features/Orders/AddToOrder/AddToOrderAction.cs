using System;
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

        [HttpPost("/order/{restaurantId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] AddToOrderRequest request)
        {
            var command = new AddToOrderCommand()
            {
                RestaurantId = restaurantId,
                MenuItemId = request.MenuItemId,
                Quantity = request.Quantity,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
