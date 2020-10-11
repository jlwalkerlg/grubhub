using System.Threading.Tasks;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Menus
{
    public interface IMenuRepository
    {
        Task Add(Menu menu);
        Task<Menu> GetByRestaurantId(RestaurantId id);
    }
}
