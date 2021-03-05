using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Baskets.RemoveFromBasket
{
    public class RemoveFromBasketAction : Action
    {
        private readonly ISender sender;

        public RemoveFromBasketAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpDelete("/restaurants/{restaurantId:guid}/basket/items/{menuItemId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid menuItemId)
        {
            var command = new RemoveFromBasketCommand()
            {
                RestaurantId = restaurantId,
                MenuItemId = menuItemId,
            };

            var result = await sender.Send(command);

            return result ? StatusCode(204) : Problem(result.Error);
        }
    }
}
