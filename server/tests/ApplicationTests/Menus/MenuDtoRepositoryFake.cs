using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Menus;

namespace ApplicationTests.Menus
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
