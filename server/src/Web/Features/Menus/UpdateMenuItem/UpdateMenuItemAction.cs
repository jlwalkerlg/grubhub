using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Domain.Users;

namespace Web.Features.Menus.UpdateMenuItem
{
    public class UpdateMenuItemAction : Action
    {
        private readonly ISender sender;

        public UpdateMenuItemAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/restaurants/{restaurantId:guid}/menu/categories/{categoryId:guid}/items/{itemId:guid}")]
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

            return result ? Ok() : Problem(result.Error);
        }
    }
}
