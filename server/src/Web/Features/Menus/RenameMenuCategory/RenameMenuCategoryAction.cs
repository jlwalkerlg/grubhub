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

        [HttpPut("/restaurants/{restaurantId:guid}/menu/categories/{categoryId:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] Guid categoryId,
            [FromBody] RenameMenuCategoryRequest request)
        {
            var command = new RenameMenuCategoryCommand
            {
                RestaurantId = restaurantId,
                CategoryId = categoryId,
                NewName = request.NewName,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
