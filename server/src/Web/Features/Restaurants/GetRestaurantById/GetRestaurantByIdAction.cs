using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Restaurants.GetRestaurantById
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

            return result ? Ok(result.Value) : Error(result.Error);
        }
    }
}
