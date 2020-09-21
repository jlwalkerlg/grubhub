using System;
using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Restaurants.GetRestaurantByManagerId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.GetByManagerId
{
    public class GetByManagerIdAction : Action
    {
        private readonly IMediator mediator;

        public GetByManagerIdAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/managers/{managerId}/restaurant")]
        public async Task<IActionResult> Execute([FromRoute] Guid managerId)
        {
            var query = new GetRestaurantByManagerIdQuery
            {
                ManagerId = managerId
            };

            var restaurant = (await mediator.Send(query)).Value;

            return Ok(new DataEnvelope
            {
                Data = restaurant
            });
        }
    }
}
