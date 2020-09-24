using System;
using System.Threading.Tasks;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Users
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
    }
}
