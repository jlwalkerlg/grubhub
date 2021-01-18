using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemAction : Action
    {
        private readonly ISender sender;

        public UpdateMenuItemAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/restaurants/{restaurantId}/menu/categories/{categoryId}/items/{itemId}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid categoryId,
            [FromRoute] Guid itemId,
            [FromBody] UpdateMenuItemRequest request)
        {
            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryId = categoryId,
                ItemId = itemId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
