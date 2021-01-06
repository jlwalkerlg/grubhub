using System;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Users.Logout;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Users.Logout
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

            var result = await handler.Handle(new LogoutCommand(), default);

            Assert.True(result);
            Assert.False(authenticatorSpy.IsAuthenticated);
        }
    }
}
