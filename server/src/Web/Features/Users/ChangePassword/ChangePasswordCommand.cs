namespace Web.Features.Users.ChangePassword
{
    public record ChangePasswordCommand : IRequest
    {
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
    }
}
