using System;
using FoodSnap.Web.Services.Authentication;

namespace FoodSnap.WebTests.Doubles
{
    public class AuthenticatorSpy : IAuthenticator
    {
        public Guid? UserId { get; set; } = null;

        public Guid? GetUserId()
        {
            return UserId;
        }
    }
}
