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

        public UserRole? UserRole { get; private set; }

        public Task SignIn(UserRole role = Web.Domain.Users.UserRole.Customer)
        {
            UserId = new UserId(Guid.NewGuid());
            return Task.CompletedTask;
        }

        public Task SignIn(Guid userId, UserRole role = Web.Domain.Users.UserRole.Customer)
        {
            UserId = new UserId(userId);
            UserRole = role;
            return Task.CompletedTask;
        }

        public Task SignIn(UserId userId, UserRole role = Web.Domain.Users.UserRole.Customer)
        {
            UserId = userId;
            UserRole = role;
            return Task.CompletedTask;
        }

        public Task SignIn(User user)
        {
            UserId = user.Id;
            UserRole = user.Role;
            return Task.CompletedTask;
        }

        public Task SignOut()
        {
            UserId = null;
            UserRole = null;
            return Task.CompletedTask;
        }
    }
}
