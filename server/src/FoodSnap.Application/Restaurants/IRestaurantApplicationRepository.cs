using System.Threading.Tasks;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Restaurants
{
    public interface IRestaurantApplicationRepository
    {
        Task Add(RestaurantApplication application);
    }
}
