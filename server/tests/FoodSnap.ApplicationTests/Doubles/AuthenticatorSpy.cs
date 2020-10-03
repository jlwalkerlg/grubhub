using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Users;

namespace FoodSnap.ApplicationTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public User User { get; set; }

        public bool IsAuthenticated => User != null;

        public UserId UserId => User?.Id;

        public void SignIn(User user)
        {
            User = user;
        }

        public void SignOut()
        {
            User = null;
        }
    }
}
