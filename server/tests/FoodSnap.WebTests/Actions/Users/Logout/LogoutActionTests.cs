using FoodSnap.Web.Actions.Users.Logout;
using FoodSnap.Web.Queries.Users;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Logout
{
    public class LogoutActionTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly LogoutAction action;

        public LogoutActionTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            action = new LogoutAction(authenticatorSpy);
        }

        [Fact]
        public void It_Signs_The_User_Out()
        {
            authenticatorSpy.User = new UserDto();

            var response = action.Execute() as StatusCodeResult;

            Assert.Null(authenticatorSpy.User);
            Assert.Equal(200, response.StatusCode);
        }
    }
}
