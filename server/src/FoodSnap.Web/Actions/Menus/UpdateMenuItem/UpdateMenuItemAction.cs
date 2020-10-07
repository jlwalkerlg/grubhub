using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.UpdateMenuItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.UpdateMenuItem
{
    public class UpdateMenuItemAction : Action
    {
        private readonly IMediator mediator;

        public UpdateMenuItemAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("/menus/{menuId}/categories/{category}/items/{item}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid menuId,
            [FromRoute] string category,
            [FromRoute] string item,
            [FromBody] UpdateMenuItemRequest request)
        {
            var command = new UpdateMenuItemCommand
            {
                MenuId = menuId,
                CategoryName = category,
                OldItemName = item,
                NewItemName = request.Name,
                Description = request.Description,
                Price = request.Price
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
