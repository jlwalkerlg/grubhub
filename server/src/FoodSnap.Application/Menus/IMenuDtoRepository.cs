using System;
using System.Threading.Tasks;

namespace FoodSnap.Application.Menus
{
    public interface IMenuDtoRepository
    {
        Task<MenuDto> GetByRestaurantId(Guid restaurantId);
    }
}
