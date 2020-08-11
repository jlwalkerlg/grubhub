using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.ApplicationTests.Restaurants
{
    public class RestaurantRepositorySpy : IRestaurantRepository
    {
        public List<Restaurant> Restaurants = new List<Restaurant>();

        public Task Add(Restaurant restaurant)
        {
            Restaurants.Add(restaurant);
            return Task.CompletedTask;
        }
    }
}
