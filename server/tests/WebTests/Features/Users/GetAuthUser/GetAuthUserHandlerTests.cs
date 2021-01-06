using System;
using System.Threading.Tasks;
using Web.Domain.Users;
using Web.Features.Users;
using Web.Features.Users.GetAuthUser;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

namespace WebTests.Features.Users.GetAuthUser
{
    public class GetAuthUserHandlerTests
    {
        private readonly UserDtoRepositoryFake userDtoRepositoryFake;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly GetAuthUserHandler handler;

        public GetAuthUserHandlerTests()
        {
            userDtoRepositoryFake = new UserDtoRepositoryFake();

            authenticatorSpy = new AuthenticatorSpy();

            handler = new GetAuthUserHandler(authenticatorSpy, userDtoRepositoryFake);
        }

        [Fact]
        public async Task It_Returns_The_Authenticated_User()
        {
            var user = new UserDto { Id = Guid.NewGuid() };
            userDtoRepositoryFake.Users.Add(user);
            authenticatorSpy.SignIn(new UserId(user.Id));

            var query = new GetAuthUserQuery();
            var result = await handler.Handle(query, default);

            Assert.True(result.IsSuccess);
            Assert.Same(user, result.Value);
        }

        [Fact]
        public async Task It_Fails_If_User_Not_Found()
        {
            var user = new UserDto { Id = Guid.NewGuid() };
            authenticatorSpy.SignIn(new UserId(user.Id));

            var query = new GetAuthUserQuery();
            var result = await handler.Handle(query, default);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}
