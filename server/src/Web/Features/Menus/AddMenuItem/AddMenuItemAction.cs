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

        [HttpPost("/restaurants/{restaurantId:guid}/menu/categories/{categoryId:guid}/items")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid categoryId,
            [FromBody] AddMenuItemRequest request)
        {
            var command = new AddMenuItemCommand()
            {
                RestaurantId = restaurantId,
                CategoryId = categoryId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            var result = await sender.Send(command);

            return result ? StatusCode(201) : Error(result.Error);
        }
    }
}
