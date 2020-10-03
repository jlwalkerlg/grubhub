using System.Threading.Tasks;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Users
{
    public interface IUserRepository
    {
        Task<User> GetById(UserId id);
        Task<User> GetByEmail(string email);
        Task<bool> EmailExists(string email);
    }
}
