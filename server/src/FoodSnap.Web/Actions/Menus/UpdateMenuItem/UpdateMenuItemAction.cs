using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.UpdateMenuItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.UpdateMenuItem
{
    public class UpdateMenuItemAction : Action
    {
        private readonly ISender sender;

        public UpdateMenuItemAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/restaurants/{restaurantId}/menu/categories/{category}/items/{item}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] string category,
            [FromRoute] string item,
            [FromBody] UpdateMenuItemRequest request)
        {
            var command = new UpdateMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
