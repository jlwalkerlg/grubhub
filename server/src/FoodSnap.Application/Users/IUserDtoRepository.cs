using System;
using System.Threading.Tasks;

namespace FoodSnap.Application.Users
{
    public interface IUserDtoRepository
    {
        Task<UserDto> GetById(Guid id);
    }
}
