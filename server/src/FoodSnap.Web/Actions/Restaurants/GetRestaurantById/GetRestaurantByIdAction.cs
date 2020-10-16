using System;
using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FoodSnap.Shared;

namespace FoodSnap.Web.Actions.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdAction : Action
    {
        private readonly ISender sender;

        public GetRestaurantByIdAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/restaurants/{id}")]
        public async Task<IActionResult> Execute([FromRoute] Guid id)
        {
            var query = new GetRestaurantByIdQuery
            {
                Id = id
            };

            var result = await sender.Send(query);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            if (result.Value == null)
            {
                return PresentError(Error.NotFound("Restaurant not found."));
            }

            return Ok(new DataEnvelope<RestaurantDto>
            {
                Data = result.Value
            });
        }
    }
}
