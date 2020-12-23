using Domain.Users;

namespace Application.Services.Authentication
{
    public interface IAuthenticator
    {
        void SignIn(User user);
        void SignOut();
        bool IsAuthenticated { get; }
        UserId UserId { get; }
    }
}
