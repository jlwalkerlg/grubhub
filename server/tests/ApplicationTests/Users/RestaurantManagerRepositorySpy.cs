using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users;
using Domain.Users;

namespace ApplicationTests.Users
{
    public class RestaurantManagerRepositorySpy : IRestaurantManagerRepository
    {
        public List<RestaurantManager> Managers = new();

        public Task Add(RestaurantManager manager)
        {
            Managers.Add(manager);
            return Task.CompletedTask;
        }
    }
}
