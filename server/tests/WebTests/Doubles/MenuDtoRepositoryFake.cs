using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Menus;

namespace WebTests.Doubles
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
