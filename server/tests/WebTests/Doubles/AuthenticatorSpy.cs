using System;
using Web.Domain.Users;
using Web.Services.Authentication;

namespace WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public bool IsAuthenticated => UserId != null;

        public UserId UserId { get; private set; }

        public void SignIn(UserId userId)
        {
            UserId = userId;
        }

        public void SignIn(Guid userId)
        {
            SignIn(new UserId(userId));
        }

        public void SignIn(User user)
        {
            SignIn(user.Id);
        }

        public void SignOut()
        {
            UserId = null;
        }
    }
}
