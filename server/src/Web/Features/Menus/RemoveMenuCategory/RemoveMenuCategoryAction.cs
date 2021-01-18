using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public RemoveMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpDelete("/restaurants/{restaurantId}/menu/categories/{categoryId}")]
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

            return result ? StatusCode(204) : Error(result.Error);
        }
    }
}
