using System.Threading.Tasks;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Restaurants
{
    public interface IRestaurantManagerRepository
    {
        Task Add(RestaurantManager manager);
        Task<bool> EmailExists(string email);
    }
}
