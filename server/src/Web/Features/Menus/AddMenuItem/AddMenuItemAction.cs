using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Menus.AddMenuItem
{
    public class AddMenuItemAction : Action
    {
        private readonly ISender sender;

        public AddMenuItemAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/restaurants/{restaurantId}/menu/items")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] AddMenuItemRequest request)
        {
            var command = new AddMenuItemCommand
            {
                RestaurantId = restaurantId,
                CategoryName = request.CategoryName,
                ItemName = request.ItemName,
                Description = request.Description,
                Price = request.Price
            };

            var result = await sender.Send(command);

            if (!result)
            {
                return Error(result.Error);
            }

            return StatusCode(201);
        }
    }
}
