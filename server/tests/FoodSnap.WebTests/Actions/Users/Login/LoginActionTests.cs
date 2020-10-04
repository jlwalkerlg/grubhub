using System.Threading.Tasks;
using FoodSnap.Application.Users.Login;
using FoodSnap.Domain;
using FoodSnap.Web.Actions.Users.Login;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Login
{
    public class LoginActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly LoginAction action;

        public LoginActionTests()
        {
            mediatorSpy = new MediatorSpy();

            action = new LoginAction(mediatorSpy);
        }

        [Fact]
        public async Task It_Returns_200_On_Success()
        {
            mediatorSpy.Result = Result.Ok();

            var command = new LoginCommand
            {
                Email = "walker.jlg@gmail.com",
                Password = "password123"
            };

            var result = await action.Login(command) as StatusCodeResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
