using System;
using Web.Services.Authentication;
using Web.Domain.Users;

namespace ApplicationTests.Services.Authentication
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
