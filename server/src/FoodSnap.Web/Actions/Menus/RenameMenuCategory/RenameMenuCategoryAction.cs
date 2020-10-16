using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RenameMenuCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.RenameMenuCategory
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

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
