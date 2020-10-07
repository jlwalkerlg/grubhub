using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.AddMenuItem
{
    public class AddMenuItemAction : Action
    {
        private readonly IMediator mediator;

        public AddMenuItemAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/menus/{menuId}/items")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid menuId,
            [FromBody] AddMenuItemRequest request)
        {
            var command = new AddMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = request.CategoryName,
                ItemName = request.ItemName,
                Description = request.Description,
                Price = request.Price
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
