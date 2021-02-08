using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Menus.RemoveMenuItem
{
    public class RemoveMenuItemAction : Action
    {
        private readonly ISender sender;

        public RemoveMenuItemAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpDelete("/restaurants/{restaurantId:guid}/menu/categories/{categoryId:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid categoryId,
            [FromRoute] Guid itemId)
        {
            var command = new RemoveMenuItemCommand()
            {
                RestaurantId = restaurantId,
                CategoryId = categoryId,
                ItemId = itemId,
            };

            var result = await sender.Send(command);

            return result ? StatusCode(204) : Error(result.Error);
        }
    }
}
