using System;
using System.Threading.Tasks;

namespace Web.Features.Menus
{
    public interface IMenuDtoRepository
    {
        Task<MenuDto> GetByRestaurantId(Guid restaurantId);
    }
}
