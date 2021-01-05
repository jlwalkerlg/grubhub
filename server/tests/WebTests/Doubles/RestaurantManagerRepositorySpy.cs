using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Features.Users;

namespace WebTests.Doubles
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
