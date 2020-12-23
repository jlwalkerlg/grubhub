using Application.Services.Authentication;

namespace Application.Users.GetAuthUser
{
    [Authenticate]
    public record GetAuthUserQuery : IRequest<UserDto>
    {
    }
}
