using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.AddMenuItem;
using FoodSnap.Web.Envelopes;
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

        [HttpPost("/menus/{menuId}/categories/{categoryId}/items")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid menuId,
            [FromRoute] Guid categoryId,
            [FromBody] AddMenuItemRequest request)
        {
            var command = new AddMenuItemCommand
            {
                MenuId = menuId,
                CategoryId = categoryId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            var response = Ok(new DataEnvelope
            {
                Data = result.Value
            });
            response.StatusCode = 201;
            return response;
        }
    }
}
