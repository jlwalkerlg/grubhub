using System.Threading.Tasks;
using Domain.Users;

namespace Application.Users
{
    public interface IUserRepository
    {
        Task<User> GetById(UserId id);
        Task<User> GetByEmail(string email);
        Task<bool> EmailExists(string email);
    }
}
