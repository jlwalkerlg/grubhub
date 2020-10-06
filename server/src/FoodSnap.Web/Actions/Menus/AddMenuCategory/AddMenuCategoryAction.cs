using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.AddMenuCategory
{
    public class AddMenuCategoryAction : Action
    {
        private readonly IMediator mediator;

        public AddMenuCategoryAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/menus/{menuId}/categories")]
        public async Task<IActionResult> Execute([FromRoute] Guid menuId, [FromBody] AddMenuCategoryRequest request)
        {
            var command = new AddMenuCategoryCommand
            {
                MenuId = menuId,
                Name = request.Name,
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return StatusCode(201);
        }
    }
}
