using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Logout;
using ApplicationTests.Services.Authentication;
using Domain;
using Domain.Users;
using Xunit;

namespace ApplicationTests.Users.Logout
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
            authenticatorSpy.SignIn(user);

            var result = await handler.Handle(new LogoutCommand(), default(CancellationToken));

            Assert.True(result.IsSuccess);
            Assert.False(authenticatorSpy.IsAuthenticated);
        }
    }
}
