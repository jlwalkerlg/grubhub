using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.ApplicationTests.Doubles;
using Xunit;
using MediatR;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Users;
using FoodSnap.Domain;
using static FoodSnap.Application.Error;

namespace FoodSnap.ApplicationTests.Services.Authentication
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
            authenticatorSpy.User = new RestaurantManager(
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var handlerResult = Result.Ok();
            RequestHandlerDelegate<Result> next = () => Task.FromResult(handlerResult);

            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                default(CancellationToken),
                next);

            Assert.True(result.IsSuccess);
            Assert.Same(handlerResult, result);
        }

        [Fact]
        public async Task It_Returns_An_Authentication_Error_If_Authentication_Fails()
        {
            authenticatorSpy.User = null;

            RequestHandlerDelegate<Result> next = () => Task.FromResult(Result.Ok());

            var result = await middleware.Handle(
                new RequireAuthenticationCommand(),
                default(CancellationToken),
                next);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Unauthenticated, result.Error.Type);
        }
    }
}
