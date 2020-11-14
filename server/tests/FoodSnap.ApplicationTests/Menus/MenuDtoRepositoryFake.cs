using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Menus;

namespace FoodSnap.ApplicationTests.Menus
{
    public class MenuDtoRepositoryFake : IMenuDtoRepository
    {
        public List<MenuDto> Menus { get; } = new();

        public Task<MenuDto> GetByRestaurantId(Guid restaurantId)
        {
            return Task.FromResult(Menus.FirstOrDefault(x => x.RestaurantId == restaurantId));
        }
    }
}
