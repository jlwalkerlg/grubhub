using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public class SearchRestaurantsAction : Action
    {
        private readonly ISender sender;
        private readonly IHostEnvironment env;

        public SearchRestaurantsAction(ISender sender, IHostEnvironment env)
        {
            this.sender = sender;
            this.env = env;
        }

        [HttpGet("/restaurants")]
        public async Task<IActionResult> Execute(
            [FromQuery] string postcode,
            [FromQuery(Name = "sort_by")] string sortBy,
            [FromQuery] string cuisines,
            [FromQuery] int? page,
            [FromQuery] int? perPage)
        {
            var query = new SearchRestaurantsQuery()
            {
                Postcode = postcode,
                Options = new RestaurantSearchOptions()
                {
                    SortBy = sortBy,
                    Cuisines = cuisines?.Split(',').ToList() ?? new(),
                    Page = page,
                    PerPage = perPage,
                },
            };

            var (data, error) = await sender.Send(query);

            return error ? Problem(error) : Ok(data);
        }
    }
}
