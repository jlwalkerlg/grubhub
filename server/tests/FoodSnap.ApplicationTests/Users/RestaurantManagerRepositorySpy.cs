using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;

namespace FoodSnap.ApplicationTests.Users
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
