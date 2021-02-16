using System.Threading.Tasks;
using Web.Domain.Users;

namespace Web.Features.Users
{
    public interface IUserRepository
    {
        Task<User> GetById(UserId id);
        Task<RestaurantManager> GetManagerById(UserId id);
        Task<User> GetByEmail(string email);
        Task<bool> EmailExists(string email);
        Task Add(User user);
    }
}
