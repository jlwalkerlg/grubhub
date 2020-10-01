using System;
using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Actions.Restaurants.GetRestaurantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdAction : Action
    {
        private readonly IMediator mediator;

        public GetRestaurantByIdAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/restaurants/{id}")]
        public async Task<IActionResult> Execute([FromRoute] Guid id)
        {
            var query = new GetRestaurantByIdQuery
            {
                Id = id
            };

            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok(new DataEnvelope
            {
                Data = result.Value
            });
        }
    }
}
