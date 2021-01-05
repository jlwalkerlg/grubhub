using Web.Domain.Users;

namespace Web.Services.Authentication
{
    public interface IAuthenticator
    {
        void SignIn(User user);
        void SignOut();
        bool IsAuthenticated { get; }
        UserId UserId { get; }
    }
}
