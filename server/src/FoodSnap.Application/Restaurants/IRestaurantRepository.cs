using System.Threading.Tasks;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Restaurants
{
    public interface IRestaurantRepository
    {
        Task Add(Restaurant restaurant);
        Task<Restaurant> GetById(RestaurantId id);
    }
}
