using System.Collections.Generic;

namespace Application.Restaurants.SearchRestaurants
{
    public record SearchRestaurantsQuery : IRequest<List<RestaurantDto>>
    {
        public string Postcode { get; init; }
        public RestaurantSearchOptions Options { get; init; }
    }
}
