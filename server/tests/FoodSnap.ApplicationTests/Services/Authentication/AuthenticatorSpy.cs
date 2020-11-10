using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Users;

namespace FoodSnap.ApplicationTests.Services.Authentication
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public bool IsAuthenticated => UserId != null;

        public UserId UserId { get; private set; }

        public void SignIn(UserId userId)
        {
            UserId = userId;
        }

        public void SignIn(User user)
        {
            UserId = user.Id;
        }

        public void SignOut()
        {
            UserId = null;
        }
    }
}
