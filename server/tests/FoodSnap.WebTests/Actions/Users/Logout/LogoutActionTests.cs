using System.Threading.Tasks;
using FoodSnap.Shared;
using FoodSnap.Web.Actions.Users.Logout;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Logout
{
    public class LogoutActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly LogoutAction action;

        public LogoutActionTests()
        {
            mediatorSpy = new MediatorSpy();

            action = new LogoutAction(mediatorSpy);
        }

        [Fact]
        public async Task It_Returns_200_On_Success()
        {
            mediatorSpy.Result = Result.Ok();

            var result = await action.Execute() as StatusCodeResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
