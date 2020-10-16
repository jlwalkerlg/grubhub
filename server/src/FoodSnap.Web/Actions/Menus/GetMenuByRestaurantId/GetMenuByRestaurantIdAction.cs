using System;
using System.Threading.Tasks;
using FoodSnap.Shared;
using FoodSnap.Web.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdAction : Action
    {
        private readonly IMediator mediator;

        public GetMenuByRestaurantIdAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/restaurants/{restaurantId}/menu")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new GetMenuByRestaurantIdQuery
            {
                RestaurantId = restaurantId,
            };

            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            // TODO: move to handler
            if (result.Value == null)
            {
                return PresentError(Error.NotFound("Menu not found."));
            }

            return Ok(new DataEnvelope<MenuDto>
            {
                Data = result.Value
            });
        }
    }
}
