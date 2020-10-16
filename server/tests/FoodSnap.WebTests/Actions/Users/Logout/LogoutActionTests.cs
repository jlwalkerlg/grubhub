using System.Threading.Tasks;
using FoodSnap.Shared;
using FoodSnap.Web.Actions.Users.Logout;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Users.Logout
{
    public class LogoutActionTests
    {
        private readonly SenderSpy senderSpy;
        private readonly LogoutAction action;

        public LogoutActionTests()
        {
            senderSpy = new SenderSpy();

            action = new LogoutAction(senderSpy);
        }

        [Fact]
        public async Task It_Returns_200_On_Success()
        {
            senderSpy.Result = Result.Ok();

            var result = await action.Execute() as StatusCodeResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
