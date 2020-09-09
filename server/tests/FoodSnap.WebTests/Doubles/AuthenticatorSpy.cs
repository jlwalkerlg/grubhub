using System;
using FoodSnap.Web.Queries.Users;
using FoodSnap.Web.Services.Authentication;

namespace FoodSnap.WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public UserDto User { get; set; }

        public Guid? GetUserId()
        {
            return User?.Id;
        }

        public void SignIn(UserDto user)
        {
            User = user;
        }
    }
}
