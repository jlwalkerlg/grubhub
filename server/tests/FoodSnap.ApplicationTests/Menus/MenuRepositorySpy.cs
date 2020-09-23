using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Menus;
using FoodSnap.Domain.Menus;

namespace FoodSnap.ApplicationTests.Menus
{
    public class MenuRepositorySpy : IMenuRepository
    {
        public List<Menu> Menus { get; } = new List<Menu>();

        public Task Add(Menu menu)
        {
            Menus.Add(menu);
            return Task.CompletedTask;
        }
    }
}