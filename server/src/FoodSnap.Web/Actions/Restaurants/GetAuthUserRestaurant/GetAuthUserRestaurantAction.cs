using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Restaurants.GetRestaurantByManagerId;
using FoodSnap.Web.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.GetAuthUserRestaurant
{
    public class GetAuthUserRestaurantAction : Action
    {
        private readonly IMediator mediator;
        private readonly IAuthenticator authenticator;

        public GetAuthUserRestaurantAction(IMediator mediator, IAuthenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        [HttpGet("/auth/restaurant/details")]
        public async Task<IActionResult> Execute()
        {
            var id = authenticator.GetUserId();

            if (id is null)
            {
                return StatusCode(401);
            }

            var query = new GetRestaurantByManagerIdQuery(id.Value);
            var restaurant = (await mediator.Send(query)).Value;

            return Ok(new DataEnvelope(restaurant));
        }
    }
}
