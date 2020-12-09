using System.Collections.Generic;

namespace FoodSnap.Application.Restaurants.SearchRestaurants
{
    public record SearchRestaurantsQuery : IRequest<List<RestaurantDto>>
    {
        public string Postcode { get; init; }
    }
}
