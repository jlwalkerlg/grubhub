using Application.Services.Authentication;

namespace Application.Users.UpdateAuthUserDetails
{
    [Authenticate]
    public record UpdateAuthUserDetailsCommand : IRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
    }
}
