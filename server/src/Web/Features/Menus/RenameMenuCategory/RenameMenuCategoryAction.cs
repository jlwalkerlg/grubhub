using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Domain.Users;

namespace Web.Features.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryAction : Action
    {
        private readonly ISender sender;

        public RenameMenuCategoryAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
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

            return result ? Ok() : Problem(result.Error);
        }
    }
}
