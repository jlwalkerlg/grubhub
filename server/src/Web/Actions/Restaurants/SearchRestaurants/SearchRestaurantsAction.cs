using System.Threading.Tasks;
using Application.Restaurants.SearchRestaurants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Actions.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsAction : Action
    {
        private readonly ISender sender;

        public SearchRestaurantsAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/restaurants")]
        public async Task<IActionResult> Execute([FromQuery] string postcode)
        {
            var query = new SearchRestaurantsQuery()
            {
                Postcode = postcode
            };

            var result = await sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}
