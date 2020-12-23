using System.Threading.Tasks;
using Domain.Restaurants;

namespace Application.Restaurants
{
    public interface IRestaurantRepository
    {
        Task Add(Restaurant restaurant);
        Task<Restaurant> GetById(RestaurantId id);
    }
}
