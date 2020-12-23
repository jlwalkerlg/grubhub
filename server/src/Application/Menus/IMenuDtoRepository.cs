using System;
using System.Threading.Tasks;

namespace Application.Menus
{
    public interface IMenuDtoRepository
    {
        Task<MenuDto> GetByRestaurantId(Guid restaurantId);
    }
}
