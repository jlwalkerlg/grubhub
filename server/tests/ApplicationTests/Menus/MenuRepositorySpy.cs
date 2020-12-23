using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Menus;
using Domain.Menus;
using Domain.Restaurants;

namespace ApplicationTests.Menus
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
