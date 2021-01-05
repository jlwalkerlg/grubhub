using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Features.Menus;

namespace WebTests.Doubles
{
    public class MenuRepositorySpy : IMenuRepository
    {
        public List<Menu> Menus { get; } = new();

        public Task Add(Menu menu)
        {
            Menus.Add(menu);
            return Task.CompletedTask;
        }

        public Task<Menu> GetByRestaurantId(RestaurantId id)
        {
            return Task.FromResult(Menus.FirstOrDefault(x => x.RestaurantId == id));
        }
    }
}
