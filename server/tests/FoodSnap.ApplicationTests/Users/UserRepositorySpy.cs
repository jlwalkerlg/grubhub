using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;
using System;

namespace FoodSnap.ApplicationTests.Users
{
    public class UserRepositorySpy : IUserRepository
    {
        public List<User> Users { get; } = new List<User>();

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(Users.FirstOrDefault(x => x.Email.Address == email));
        }

        public Task Add(User user)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User> GetById(Guid id)
        {
            return Task.FromResult(Users.FirstOrDefault(x => x.Id == id));
        }
    }
}
