using System;
using System.Threading.Tasks;
using Web.Domain.Menus;

namespace Web.Features.Menus
{
    public interface IMenuRepository
    {
        Task Add(Menu menu);
        Task<Menu> GetByRestaurantId(Guid id);
    }
}
