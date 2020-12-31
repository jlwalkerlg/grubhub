using System.Threading.Tasks;
using Application.Restaurants;
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
        public async Task<IActionResult> Execute(
            [FromQuery] string postcode,
            [FromQuery(Name = "sort_by")] string sortBy)
        {
            var query = new SearchRestaurantsQuery()
            {
                Postcode = postcode,
                Options = new RestaurantSearchOptions()
                {
                    SortBy = sortBy
                },
            };

            var result = await sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}
