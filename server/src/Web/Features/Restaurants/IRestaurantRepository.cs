using System.Threading.Tasks;
using Web.Domain.Restaurants;

namespace Web.Features.Restaurants
{
    public interface IRestaurantRepository
    {
        Task Add(Restaurant restaurant);
        Task<Restaurant> GetById(RestaurantId id);
    }
}
