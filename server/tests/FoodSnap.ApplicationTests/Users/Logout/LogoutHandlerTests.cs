using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Users.Logout;
using FoodSnap.ApplicationTests.Services.Authentication;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.ApplicationTests.Users.Logout
{
    public class LogoutHandlerTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly LogoutHandler handler;

        public LogoutHandlerTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            handler = new LogoutHandler(authenticatorSpy);
        }

        [Fact]
        public async Task It_Signs_The_User_Out()
        {
            User user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.User = user;

            var result = await handler.Handle(new LogoutCommand(), default(CancellationToken));

            Assert.True(result.IsSuccess);
            Assert.Null(authenticatorSpy.User);
        }
    }
}
