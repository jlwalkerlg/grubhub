using System;
using System.Threading.Tasks;
using Shouldly;
using Web;
using Web.Services.Authentication;
using WebTests.Doubles;
using Xunit;

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
        public async Task It_Returns_An_Authentication_Error_If_Authentication_Fails()
        {
            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                default,
                () => Task.FromResult(Result.Ok()));

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthenticated);
        }

        [Fact]
        public async Task It_Returns_The_Handler_Result_If_Authentication_Passes()
        {
            authenticatorSpy.SignIn(Guid.NewGuid());

            var handlerResult = Result.Ok();

            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                default,
                () => Task.FromResult(handlerResult));

            result.ShouldBeSuccessful();
            result.ShouldBe(handlerResult);
        }

        [Authenticate]
        public class RequireAuthenticationCommand : IRequest
        {
        }
    }
}
