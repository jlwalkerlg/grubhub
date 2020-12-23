using System.Threading.Tasks;
using Domain.Menus;
using Domain.Restaurants;

namespace Application.Menus
{
    public interface IMenuRepository
    {
        Task Add(Menu menu);
        Task<Menu> GetByRestaurantId(RestaurantId id);
    }
}
