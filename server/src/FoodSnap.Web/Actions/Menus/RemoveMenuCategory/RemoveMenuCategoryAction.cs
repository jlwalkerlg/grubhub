using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RemoveMenuCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.RemoveMenuCategory
{
    public class RemoveMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public RemoveMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpDelete("/restaurants/{restaurantId}/menu/categories/{category}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromRoute] string category)
        {
            var command = new RemoveMenuCategoryCommand
            {
                RestaurantId = restaurantId,
                CategoryName = category,
            };

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return StatusCode(204);
        }
    }
}
