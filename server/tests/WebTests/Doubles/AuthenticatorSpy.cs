using System;
using Web.Domain.Users;
using Web.Services.Authentication;

namespace WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public bool IsAuthenticated => UserId != null;

        public UserId UserId { get; private set; }

        public void SignIn()
        {
            UserId = new UserId(Guid.NewGuid());
        }

        public void SignIn(Guid userId)
        {
            UserId = new UserId(userId);
        }

        public void SignIn(User user)
        {
            UserId = user.Id;
        }

        public void SignIn(UserId userId)
        {
            UserId = userId;
        }

        public void SignOut()
        {
            UserId = null;
        }
    }
}
