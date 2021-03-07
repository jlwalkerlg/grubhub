namespace Web.Features.Users.ChangePassword
{
    public record ChangePasswordCommand : IRequest
    {
        public string Password { get; init; }
    }
}
