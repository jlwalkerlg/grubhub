using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain.Restaurants;
using Web.Features.Restaurants;

namespace WebTests.Doubles
{
    public class RestaurantRepositorySpy : IRestaurantRepository
    {
        public List<Restaurant> Restaurants = new();

        public Task Add(Restaurant restaurant)
        {
            Restaurants.Add(restaurant);
            return Task.CompletedTask;
        }

        public Task<Restaurant> GetById(RestaurantId id)
        {
            return Task.FromResult(Restaurants.FirstOrDefault(x => x.Id == id));
        }
    }
}