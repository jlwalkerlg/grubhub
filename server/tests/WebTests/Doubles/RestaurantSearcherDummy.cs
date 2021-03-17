using System.Threading.Tasks;
using Web.Domain;
using Web.Features.Restaurants.SearchRestaurants;

namespace WebTests.Doubles
{
    public class RestaurantSearcherDummy : IRestaurantSearcher
    {
        public Task<SearchRestaurantsResponse> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null)
        {
            return Task.FromResult<SearchRestaurantsResponse>(null);
        }
    }
}
