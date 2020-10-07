using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.RemoveMenuItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.RemoveMenuItem
{
    public class RemoveMenuItemAction : Action
    {
        private readonly IMediator mediator;

        public RemoveMenuItemAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete("/menus/{menuId}/categories/{category}/items/{item}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid menuId,
            [FromRoute] string category,
            [FromRoute] string item)
        {
            var command = new RemoveMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                ItemName = item,
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return StatusCode(204);
        }
    }
}
