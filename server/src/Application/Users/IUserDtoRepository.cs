using System;
using System.Threading.Tasks;

namespace Application.Users
{
    public interface IUserDtoRepository
    {
        Task<UserDto> GetById(Guid id);
    }
}
