using System.Collections.Generic;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public record SearchRestaurantsQuery : IRequest<List<RestaurantSearchResult>>
    {
        public string Postcode { get; init; }
        public RestaurantSearchOptions Options { get; init; }
    }
}
