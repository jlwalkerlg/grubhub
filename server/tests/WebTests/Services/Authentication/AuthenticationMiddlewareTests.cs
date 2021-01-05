using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Web.Services.Authentication;
using Web.Domain.Users;
using Web.Domain;
using static Web.Error;
using System;
using Web;
using WebTests.Doubles;

namespace WebTests.Services.Authentication
{
    public class AuthenticationMiddlewareTests
    {
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly AuthenticationMiddleware<RequireAuthenticationCommand, Result> middleware;

        public AuthenticationMiddlewareTests()
        {
            authenticatorSpy = new AuthenticatorSpy();

            middleware = new AuthenticationMiddleware<RequireAuthenticationCommand, Result>(authenticatorSpy);
        }

        [Fact]
        public async Task It_Returns_The_Handler_Result_If_Authentication_Passes()
        {
            var user = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");
            authenticatorSpy.SignIn(user);

            var handlerResult = Result.Ok();

            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                CancellationToken.None,
                () => Task.FromResult(handlerResult));

            Assert.True(result.IsSuccess);
            Assert.Same(handlerResult, result);
        }

        [Fact]
        public async Task It_Returns_An_Authentication_Error_If_Authentication_Fails()
        {
            authenticatorSpy.SignOut();

            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                CancellationToken.None,
                () => Task.FromResult(Result.Ok()));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthenticated, result.Error.Type);
        }

        [Authenticate]
        public class RequireAuthenticationCommand : IRequest
        {
        }
    }
}
