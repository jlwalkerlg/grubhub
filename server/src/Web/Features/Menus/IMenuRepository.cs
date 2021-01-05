using System.Threading.Tasks;
using Web.Domain.Menus;
using Web.Domain.Restaurants;

namespace Web.Features.Menus
{
    public interface IMenuRepository
    {
        Task Add(Menu menu);
        Task<Menu> GetByRestaurantId(RestaurantId id);
    }
}
