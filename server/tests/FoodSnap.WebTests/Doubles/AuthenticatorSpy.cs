using System;
using FoodSnap.Web.Queries.Users;
using FoodSnap.Web.Services.Authentication;

namespace FoodSnap.WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public UserDto User { get; set; }

        public bool IsAuthenticated => User != null;

        public Guid UserId => User.Id;

        public void SignIn(UserDto user)
        {
            User = user;
        }

        public void SignOut()
        {
            User = null;
        }
    }
}
