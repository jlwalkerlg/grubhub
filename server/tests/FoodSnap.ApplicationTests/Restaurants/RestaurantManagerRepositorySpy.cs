using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants;
using FoodSnap.Domain.Users;

namespace FoodSnap.ApplicationTests.Restaurants
{
    public class RestaurantManagerRepositorySpy : IRestaurantManagerRepository
    {
        public List<RestaurantManager> Managers = new List<RestaurantManager>();

        public Task Add(RestaurantManager manager)
        {
            Managers.Add(manager);
            return Task.CompletedTask;
        }
    }
}
