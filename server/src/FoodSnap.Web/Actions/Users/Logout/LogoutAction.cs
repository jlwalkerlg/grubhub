using FoodSnap.Web.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Logout
{
    public class LogoutAction : Action
    {
        private readonly IAuthenticator authenticator;

        public LogoutAction(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        [HttpPost("/logout")]
        public IActionResult Execute()
        {
            authenticator.SignOut();

            return Ok();
        }
    }
}
