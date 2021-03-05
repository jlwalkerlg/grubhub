using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Web.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public RemoveMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpDelete("/restaurants/{restaurantId:guid}/menu/categories/{categoryId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid categoryId)
        {
            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = restaurantId,
                CategoryId = categoryId,
            };

            var result = await sender.Send(command);

            return result ? StatusCode(204) : Problem(result.Error);
        }
    }
}
