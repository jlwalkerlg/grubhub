namespace Web.Features.Users.Register
{
    public record RegisterCommand : IRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
