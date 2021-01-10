using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public interface IRestaurantSearcher
    {
        Task<List<RestaurantSearchResult>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null);
    }
}
