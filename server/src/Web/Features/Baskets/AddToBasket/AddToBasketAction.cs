using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Baskets.AddToBasket
{
    public class AddToBasketAction : Action
    {
        private readonly ISender sender;

        public AddToBasketAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/restaurants/{restaurantId:guid}/basket")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] AddToBasketRequest request)
        {
            var command = new AddToBasketCommand()
            {
                RestaurantId = restaurantId,
                MenuItemId = request.MenuItemId,
                Quantity = request.Quantity,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
