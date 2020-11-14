using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Users.GetAuthUser
{
    [Authenticate]
    public record GetAuthUserQuery : IRequest<UserDto>
    {
    }
}
