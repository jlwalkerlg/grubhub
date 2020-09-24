using System.Threading.Tasks;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Users
{
    public interface IRestaurantManagerRepository
    {
        Task Add(RestaurantManager manager);
    }
}
