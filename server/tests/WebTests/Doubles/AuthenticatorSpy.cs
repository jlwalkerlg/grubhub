using System;
using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Services.Authentication;

namespace WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public bool IsAuthenticated => UserId != null;

        public UserId UserId { get; private set; }

        public Task SignIn()
        {
            UserId = new UserId(Guid.NewGuid());
            return Task.CompletedTask;
        }

        public Task SignIn(Guid userId)
        {
            UserId = new UserId(userId);
            return Task.CompletedTask;
        }

        public Task SignIn(User user)
        {
            UserId = user.Id;
            return Task.CompletedTask;
        }

        public Task SignIn(UserId userId)
        {
            UserId = userId;
            return Task.CompletedTask;
        }

        public Task SignOut()
        {
            UserId = null;
            return Task.CompletedTask;
        }
    }
}
