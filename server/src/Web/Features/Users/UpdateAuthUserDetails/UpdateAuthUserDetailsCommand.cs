using Web.Services.Authentication;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    [Authenticate]
    public record UpdateAuthUserDetailsCommand : IRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
    }
}
