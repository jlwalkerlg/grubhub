using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Domain.Users;

namespace Web.Features.Menus.AddMenuCategory
{
    public class AddMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public AddMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPost("/restaurants/{restaurantId:guid}/menu/categories")]
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

            if (!result)
            {
                return Problem(result.Error);
            }

            return StatusCode(201);
        }
    }
}
