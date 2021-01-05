using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Users;
using Web.Domain.Users;

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
