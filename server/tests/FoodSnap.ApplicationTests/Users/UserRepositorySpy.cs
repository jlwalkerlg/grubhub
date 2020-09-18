using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;

namespace FoodSnap.ApplicationTests.Users
{
    public class UserRepositorySpy : IUserRepository
    {
        public List<User> Users { get; } = new List<User>();

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(Users.FirstOrDefault(x => x.Email.Address == email));
        }
    }
}
