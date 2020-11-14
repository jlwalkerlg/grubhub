using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Menus;
using FoodSnap.Domain.Menus;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.ApplicationTests.Menus
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
