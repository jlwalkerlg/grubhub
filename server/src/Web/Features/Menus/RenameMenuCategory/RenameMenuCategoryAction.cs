using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public RenameMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/restaurants/{restaurantId}/menu/categories/{category}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] string category,
            [FromBody] RenameMenuCategoryRequest request)
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurantId,
                OldName = category,
                NewName = request.NewName,
            };

            var result = await sender.Send(command);

            if (!result)
            {
                return Error(result.Error);
            }

            return Ok();
        }
    }
}
