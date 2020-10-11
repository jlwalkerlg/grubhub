using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RenameMenuCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.RenameMenuCategory
{
    public class RenameMenuCategoryAction : Action
    {
        private readonly IMediator mediator;

        public RenameMenuCategoryAction(IMediator mediator)
        {
            this.mediator = mediator;
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

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
