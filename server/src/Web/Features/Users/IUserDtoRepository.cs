using System;
using System.Threading.Tasks;

namespace Web.Features.Users
{
    public interface IUserDtoRepository
    {
        Task<UserDto> GetById(Guid id);
    }
}
