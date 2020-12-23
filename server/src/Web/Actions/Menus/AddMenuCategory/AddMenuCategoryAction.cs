using System;
using System.Threading.Tasks;
using Application.Menus.AddMenuCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Actions.Menus.AddMenuCategory
{
    public class AddMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public AddMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/restaurants/{restaurantId}/menu/categories")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] AddMenuCategoryRequest request)
        {
            var command = new AddMenuCategoryCommand
            {
                RestaurantId = restaurantId,
                Name = request.Name,
            };

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return StatusCode(201);
        }
    }
}
