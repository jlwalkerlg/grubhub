using System.Threading.Tasks;
using Web.Domain;

namespace Web.Features.Restaurants.SearchRestaurants
{
    public interface IRestaurantSearcher
    {
        Task<SearchRestaurantsResponse> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null);
    }
}
