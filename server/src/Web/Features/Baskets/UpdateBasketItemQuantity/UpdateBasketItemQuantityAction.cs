using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Baskets.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityAction : Action
    {
        private readonly ISender sender;

        public UpdateBasketItemQuantityAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/restaurants/{restaurantId:guid}/basket/items/{menuItemId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid menuItemId,
            [FromBody] UpdateBasketItemQuantityRequest request)
        {
            var command = new UpdateBasketItemQuantityCommand()
            {
                RestaurantId = restaurantId,
                MenuItemId = menuItemId,
                Quantity = request.Quantity,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
