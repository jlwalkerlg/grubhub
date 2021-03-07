
namespace Web.Features.Users.UpdateAuthUserDetails
{
    public record UpdateAuthUserDetailsCommand : IRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
    }
}
