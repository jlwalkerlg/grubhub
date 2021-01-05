using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Features.Restaurants;

namespace WebTests.Doubles
{
    public class RestaurantDtoRepositoryFake : IRestaurantDtoRepository
    {
        public List<RestaurantDto> Restaurants { get; } = new();

        public Task<RestaurantDto> GetById(Guid id)
        {
            return Task.FromResult(Restaurants.FirstOrDefault(x => x.Id == id));
        }

        public Coordinates SearchCoordinates { get; private set; }

        public Task<List<RestaurantDto>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null)
        {
            SearchCoordinates = coordinates;
            return Task.FromResult(Restaurants);
        }
    }
}
