using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;

namespace FoodSnap.ApplicationTests.Restaurants
{
    public class RestaurantDtoRepositoryFake : IRestaurantDtoRepository
    {
        public List<RestaurantDto> Restaurants { get; } = new();

        public Task<RestaurantDto> GetById(Guid id)
        {
            return Task.FromResult(Restaurants.FirstOrDefault(x => x.Id == id));
        }
    }
}
