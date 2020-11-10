using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Application.Users;

namespace FoodSnap.ApplicationTests.Users
{
    public class UserDtoRepositoryFake : IUserDtoRepository
    {
        public List<UserDto> Users { get; } = new List<UserDto>();

        public Task<UserDto> GetById(Guid id)
        {
            return Task.FromResult(Users.FirstOrDefault(x => x.Id == id));
        }
    }
}
