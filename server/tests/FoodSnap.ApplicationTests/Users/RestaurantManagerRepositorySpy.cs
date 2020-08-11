using System.Collections.Generic;
using System.Linq;
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

        public Task<bool> EmailExists(string email)
        {
            return Task.FromResult(Managers.Any(x => x.Email.Address == email));
        }
    }
}
