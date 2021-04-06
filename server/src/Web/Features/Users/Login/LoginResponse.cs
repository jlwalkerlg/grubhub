namespace Web.Features.Users.Login
{
    public record LoginResponse
    {
        public string XsrfToken { get; init; }
    }
}
