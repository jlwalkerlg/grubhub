using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Users.GetAuthUser
{
    [Authenticate]
    public class GetAuthUserQuery : IRequest<UserDto>
    {
    }
}
