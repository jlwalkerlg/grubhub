using Web.Services.Authentication;

namespace Web.Features.Users.GetAuthUser
{
    [Authenticate]
    public record GetAuthUserQuery : IRequest<UserDto>
    {
    }
}
