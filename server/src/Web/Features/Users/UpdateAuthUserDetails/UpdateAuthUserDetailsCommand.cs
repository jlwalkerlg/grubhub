
namespace Web.Features.Users.UpdateAuthUserDetails
{
    public record UpdateAuthUserDetailsCommand : IRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
    }
}
