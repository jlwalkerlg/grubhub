using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Features.Restaurants.SearchRestaurants;

namespace WebTests.Doubles
{
    public class RestaurantSearcherFake : IRestaurantSearcher
    {
        public List<RestaurantSearchResult> Restaurants { get; } = new();

        public Task<RestaurantSearchResult> GetById(Guid id)
        {
            return Task.FromResult(Restaurants.FirstOrDefault(x => x.Id == id));
        }

        public Coordinates SearchCoordinates { get; private set; }

        public Task<List<RestaurantSearchResult>> Search(
            Coordinates coordinates,
            RestaurantSearchOptions options = null)
        {
            SearchCoordinates = coordinates;
            return Task.FromResult(Restaurants);
        }
    }
}
