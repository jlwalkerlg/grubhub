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

        [HttpPut("/restaurants/{restaurantId}/menu/categories/{category}/items/{itemName}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] string category,
            [FromRoute] string itemName,
            [FromBody] UpdateMenuItemRequest request)
        {
            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = itemName,
                NewItemName = request.NewItemName,
                Description = request.Description,
                Price = request.Price
            };

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return Ok();
        }
    }
}
