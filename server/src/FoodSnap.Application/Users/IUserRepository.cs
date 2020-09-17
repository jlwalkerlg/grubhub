using System.Threading.Tasks;
using FoodSnap.Domain.Users;

namespace FoodSnap.Application.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
    }
}
