using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Web.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.GetMenuByRestaurantId
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
            await Task.CompletedTask;

            var query = new GetMenuByRestaurantIdQuery
            {
                RestaurantId = restaurantId,
            };

            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            if (result.Value == null)
            {
                return PresentError(Error.NotFound("Menu not found."));
            }

            return Ok(new DataEnvelope
            {
                Data = result.Value
            });
        }
    }
}
